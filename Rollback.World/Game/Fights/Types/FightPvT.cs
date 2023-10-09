using System.Collections.Concurrent;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Teams;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Fights;
using Rollback.World.Handlers.Guilds;

namespace Rollback.World.Game.Fights.Types
{
    public sealed class FightPvT : Fight<TaxCollectorAttackerTeam, TaxCollectorDefenderTeam>
    {
        private readonly ConcurrentDictionary<int, Character> _defendersQueue;
        private readonly TaxCollector _taxCollector;
        private bool _isDefendersPlacementPhase;
        private DateTime? _defendersPlacementDateTime;

        public override FightTypeEnum Type =>
            FightTypeEnum.FIGHT_TYPE_PvT;

        public sbyte DefendersFreeSlotCount
        {
            get
            {
                var fighters = Defenders.GetFighters<FightActor>();
                return (sbyte)(FightManager.MaxFightersByTeam - _defendersQueue.Count + fighters.Length > 0 ?
                    FightManager.MaxFightersByTeam - _defendersQueue.Count + fighters.Length : 0);
            }
        }

        public override bool CanCancelFight =>
            false;

        public FightPvT(short id, MapInstance map, TaxCollectorAttackerTeam challengers, TaxCollectorDefenderTeam defenders,
            bool activateBlades, TaxCollector taxCollector)
            : base(id, map, challengers, defenders, activateBlades)
        {
            _defendersQueue = new();
            _taxCollector = taxCollector;
        }

        protected override FightResultListEntry[] GenerateResults()
        {
            var index = 0;
            var result = new FightResultListEntry[Winners.Count + Losers.Count];

            var looters = Winners.Select(x => x.Result).OrderByDescending(x => x.Prospecting).ToArray();

            var teamPP = 0;
            var challengersWin = Winners.FirstOrDefault()?.Team!.Side is TeamSide.Challenger;
            if (challengersWin)
                teamPP = looters.Where(x => x.CanDrop).Sum(x => x.Prospecting > 100 ? 100 : x.Prospecting);

            var i = 0;
            var taxItems = _taxCollector.Bag.GetItems();
            foreach (var loot in looters)
            {
                if (challengersWin && loot.CanDrop)
                {
                    loot.AddEarnedKamas(FightFormulas.CalculateEarnedKamas(_taxCollector.GatheredKamas, 0, false));

                    var count = i + (int)Math.Ceiling(taxItems.Length * ((double)loot.Prospecting / teamPP));
                    for (; i < count && i < taxItems.Length; i++)
                        loot.AddEarnedItem(taxItems[i].Id, 1);
                }

                loot.Apply();
                result[index++] = loot.GetResult(FightOutcomeEnum.RESULT_VICTORY);
            }

            foreach (var loser in Losers)
                result[index++] = loser.Result.GetResult(FightOutcomeEnum.RESULT_LOST);

            return result;
        }

        public override bool CanChangePlacementCell(FightActor fighter, short cellId) =>
            base.CanChangePlacementCell(fighter, cellId) && (fighter.Team!.Side is TeamSide.Defender || !_isDefendersPlacementPhase);

        protected override void ShowBlades()
        {
            if (!BladesVisible && Challengers.BladeCell is not null && Defenders.BladeCell is not null)
            {
                foreach (var map in _taxCollector.Map.GetInstances())
                    map.Send(FightHandler.SendGameRolePlayShowChallengeMessage, new object[] { this });

                BladesVisible = true;
            }
        }

        protected override void HideBlades()
        {
            if (BladesVisible)
            {
                foreach (var map in _taxCollector.Map.GetInstances())
                    map.Send(FightHandler.SendGameRolePlayRemoveChallengeMessage, new object[] { Id });

                BladesVisible = false;
            }
        }

        protected override int GetTimerTime(FightTimer timerType) =>
            timerType switch
            {
                FightTimer.Placement => FightConfig.Instance.PvTChallengersPlacementTime,
                FightTimer.PvTDefendersPlacement => FightConfig.Instance.PvTDefendersPlacementTime,
                _ => base.GetTimerTime(timerType),
            };

        protected override void ExecuteTimerAction(FightTimer timerType)
        {
            switch (timerType)
            {
                case FightTimer.Placement:
                    StartDefendersPlacement();
                    break;

                case FightTimer.PvTDefendersPlacement:
                    StartFight();
                    break;

                default:
                    base.ExecuteTimerAction(timerType);
                    break;
            }
        }

        private void StartDefendersPlacement()
        {
            if (State is FightState.Placement)
            {
                _defendersPlacementDateTime = DateTime.Now;
                _isDefendersPlacementPhase = true;

                if (_defendersQueue.Count is 0)
                    StartFight();
                else
                {
                    foreach (var defender in _defendersQueue.Values)
                    {
                        defender.GuildMember!.WaitingFight = default;

                        var map = defender.MapInstance;
                        defender.Teleport(Map);
                        defender.JoinTeam(Defenders)?.SetSaveMap(map, defender.Cell.Id);
                    }

                    StartTimer(FightTimer.PvTDefendersPlacement);
                }
            }
        }

        public override void StartPlacement()
        {
            base.StartPlacement();
            _taxCollector.Guild.Send(GuildHandler.SendTaxCollectorAttackedMessage, new[] { _taxCollector });
        }

        public override void CheckAllStatus() { }

        public override bool CheckFightEnd()
        {
            if (Defenders.GetFighters<TaxCollectorFighter>(x => x.Alive).Length is 0)
            {
                Winners = Challengers.GetFighters<FightActor>();
                Losers = Defenders.GetFighters<FightActor>();
            }

            return base.CheckFightEnd();
        }

        public override int GetPlacementTimeLeft()
        {
            var timeLeft = (int)(_isDefendersPlacementPhase ? FightConfig.Instance.PvTDefendersPlacementTime : FightConfig.Instance.PvTChallengersPlacementTime
                - (DateTime.Now - (_isDefendersPlacementPhase ? _defendersPlacementDateTime! : _creationDate)).Value.TotalSeconds);

            return timeLeft > 0 ? timeLeft : 0;
        }

        public int GetChallengerPlacementTimeLeft()
        {
            var timeLeft = _isDefendersPlacementPhase ? 0 :
                (int)(FightConfig.Instance.PvTChallengersPlacementTime - (DateTime.Now - _creationDate).TotalMilliseconds);

            return timeLeft > 0 ? timeLeft : 0;
        }

        public FighterRefusedReasonEnum AddWaitingDefender(Character character)
        {
            var result = FighterRefusedReasonEnum.FIGHTER_ACCEPTED;

            if (character.IsBusy || character.GuildMember?.WaitingFight is not null)
                result = FighterRefusedReasonEnum.IM_OCCUPIED;
            else if (_isDefendersPlacementPhase)
                result = FighterRefusedReasonEnum.TOO_LATE;
            else if (!_taxCollector.Guild.IsMember(character.Id))
                result = FighterRefusedReasonEnum.WRONG_GUILD;
            else if (DefendersFreeSlotCount <= 0)
                result = FighterRefusedReasonEnum.TEAM_FULL;
            else if (_defendersQueue.ContainsKey(character.Id) || _defendersQueue.Values.Any(x => x.Client.IP == character.Client.IP))
                result = FighterRefusedReasonEnum.MULTIACCOUNT_NOT_ALLOWED;
            else
            {
                _defendersQueue.TryAdd(character.Id, character);
                character.GuildMember!.WaitingFight = this;

                character.GuildMember.Guild.Send(GuildHandler.SendGuildFightPlayersHelpersJoinMessage, new object[] { character, _taxCollector });
            }

            return result;
        }

        public void RemoveWaitingDefender(int characterId)
        {
            if (_defendersQueue.TryRemove(characterId, out var character))
            {
                character.GuildMember!.WaitingFight = default;
                character.GuildMember.Guild.Send(GuildHandler.SendGuildFightPlayersHelpersLeaveMessage, new object[] { character, _taxCollector });
            }
        }
    }
}

