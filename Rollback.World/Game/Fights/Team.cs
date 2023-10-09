using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Fights;
using Rollback.World.Handlers.Maps;
using Rollback.World.Network;

namespace Rollback.World.Game.Fights
{
    public abstract class Team : ClientCollection<WorldClient, Message>
    {
        private readonly ConcurrentDictionary<int, FightActor> _fighters;

        public abstract TeamType Type { get; }

        public TeamSide Side { get; set; }

        public Cell[] PlacementCells { get; set; }

        public Cell? BladeCell { get; set; }

        public bool RestrictedToParty { get; set; }

        public bool Closed { get; set; }

        public bool AskingForHelp { get; set; }

        [NotNull]
        public IFight? Fight { get; set; }

        public AlignmentSideEnum AlignmentSide { get; }

        public FightActor? Leader { get; private set; }

        public bool Full =>
            Fight?.Started == false && (_fighters.Count >= PlacementCells.Length || _fighters.Count >= FightManager.MaxFightersByTeam);

        public Team OpposedTeam =>
            Side is TeamSide.Challenger ? Fight.Defenders : Fight.Challengers;

        public FightTeamInformations FightTeamInformations =>
            new((sbyte)Side,
                Leader is null ? 0 : Leader.Id,
                (sbyte)AlignmentSide,
                _fighters.Values.Where(x => x is not SummonedStaticMonster).Select(x => x.FightTeamMemberInformations).ToArray());

        public FightOptionsInformations FightOptionsInformations =>
            new(Fight.SpectatorClosed,
                RestrictedToParty,
                Closed,
                AskingForHelp);

        public Team(Cell[] placementCells)
        {
            _fighters = new();
            PlacementCells = placementCells;
        }

        public Team(Cell[] placementCells, AlignmentSideEnum alignmentSide) : this(placementCells) =>
            AlignmentSide = alignmentSide;

        protected override IEnumerable<WorldClient> GetClients() =>
            GetFighters<CharacterFighter>().Select(x => x.Character.Client);

        public FightActor? GetFighter(int id) =>
            _fighters.ContainsKey(id) ? _fighters[id] : default;

        public TFighter? GetFighter<TFighter>(Predicate<TFighter> p) where TFighter : FightActor =>
            (TFighter?)_fighters.Values.FirstOrDefault(x => x is TFighter tfighter && p(tfighter));

        public TFighter[] GetFighters<TFighter>(Predicate<TFighter>? p = default) where TFighter : FightActor =>
            (p is null ? _fighters.Values.Where(x => x is TFighter) : _fighters.Values.Where(x => x is TFighter tFighter && p(tFighter))).Select(x => (TFighter)x).ToArray();

        public bool GetOptionState(FightOptionsEnum option) =>
            option switch
            {
                FightOptionsEnum.FIGHT_OPTION_SET_SECRET => false, // TO DO spectate
                FightOptionsEnum.FIGHT_OPTION_SET_CLOSED => Closed,
                FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY => RestrictedToParty,
                FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP => AskingForHelp,
                _ => throw new Exception("Unknown team option...")
            };

        #region Fighters
        public bool AddFighter(FightActor fighter)
        {
            var result = false;

            if (!Full)
            {
                if (!_fighters.ContainsKey(fighter.Id))
                {
                    if (!Fight.Started)
                    {
                        if (GetRandomFreePlacementCell() is { } placementCell)
                        {
                            if (Leader is null)
                            {
                                Leader = fighter;
                                BladeCell = fighter.Cell;
                            }

                            fighter.Cell = placementCell;
                        }
                    }

                    if (!Fight.IsCellFreeToWalkOn(fighter.Cell.Id))
                        fighter.Cell = Fight.GetFirstFreeCellNear(fighter.Cell.Point)!;

                    fighter.Direction = GetFighterOrientation(fighter);
                    fighter.Team = this;

                    _fighters.TryAdd(fighter.Id, fighter);

                    fighter.OnJoinFight();
                    Fight.OnFighterAdded(fighter);

                    UpdateBlades();

                    OpposedTeam.UpdateFightersOrientation();
                    result = true;
                }
            }

            return result;
        }

        public void KickFighter(CharacterFighter kicker, int targetId)
        {
            if (!Fight.Started && _fighters.ContainsKey(targetId) && kicker.CanKick(_fighters[targetId]))
                RemoveActor(_fighters[targetId]);
        }

        public void RemoveActor(FightActor fighter)
        {
            if (_fighters.Remove(fighter.Id, out _))
            {
                switch (Fight.State)
                {
                    case FightState.Placement:
                        Fight.Send(FightHandler.SendGameFightRemoveTeamMemberMessage, new object[] { fighter });
                        break;

                    default:
                        if (fighter.Alive)
                            Fight.Send(MapHandler.SendGameContextRemoveElementMessage, new object[] { fighter.Id });
                        break;
                }

                Fight.Send(FightHandler.SendGameFightRemoveTeamMemberMessage, new object[] { fighter });

                fighter.OnQuitFight();
                Fight.OnFighterRemoved(fighter);

                UpdateBlades();
            }
        }

        public virtual FighterRefusedReasonEnum CanJoin(Character character)
        {
            var result = FighterRefusedReasonEnum.FIGHTER_ACCEPTED;

            if (Full)
                result = FighterRefusedReasonEnum.TEAM_FULL;
            else if (AlignmentSide is not AlignmentSideEnum.ALIGNMENT_WITHOUT && AlignmentSide != character.AlignmentSide)
                result = FighterRefusedReasonEnum.WRONG_ALIGNMENT;
            else if (Fight.Started)
                result = FighterRefusedReasonEnum.TOO_LATE;
            else if (Fight.Map != character.MapInstance)
                result = FighterRefusedReasonEnum.WRONG_MAP;
            else if (character.IsBusy)
                result = FighterRefusedReasonEnum.IM_OCCUPIED;
            else if (Closed)
                result = FighterRefusedReasonEnum.TEAM_LIMITED_BY_MAINCHARACTER;
            else if (character.IsDead)
                result = FighterRefusedReasonEnum.GHOST_REFUSED;

            return result;
        }
        #endregion

        #region Placement
        private Cell? GetRandomFreePlacementCell()
        {
            var freeCells = PlacementCells.Where(x => _fighters.All(y => y.Value.Cell.Id != x.Id)).ToArray();

            return freeCells.Length is 0 ? null : freeCells[Random.Shared.Next(freeCells.Length)];
        }

        public DirectionsEnum GetFighterOrientation(FightActor fighter)
        {
            var nearestCell = default(KeyValuePair<Cell, uint>?);

            foreach (var ennemy in OpposedTeam.GetFighters<FightActor>(x => x.Alive))
            {
                var distance = fighter.Cell.Point.ManhattanDistanceTo(ennemy.Cell.Point);

                if (nearestCell is null || distance < nearestCell.Value.Value)
                    nearestCell = new(ennemy.Cell, distance);
            }

            return nearestCell is null ? fighter.Direction : fighter.Cell.Point.OrientationTo(nearestCell.Value.Key.Point, false);
        }

        public void UpdateFightersOrientation()
        {
            foreach (var fighter in _fighters.Values)
                fighter.Direction = GetFighterOrientation(fighter);

            Fight.Send(FightHandler.SendGameEntitiesDispositionMessage, new object[] { Fight.GetAllFighters<FightActor>() });
        }

        private void UpdateBlades()
        {
            if (Fight.BladesVisible)
                Fight.Map.Send(FightHandler.SendGameFightUpdateTeamMessage, new object[] { this });
        }

        public bool CanChangePlacementCell(FightActor fighter, short cellId) =>
            Fight.State is FightState.Placement && PlacementCells.Any(x => x.Id == cellId) && _fighters.Values.All(x => x.Cell.Id != cellId) &&
            Fight.CanChangePlacementCell(fighter, cellId);

        public void ToggleOption(FightOptionsEnum option)
        {
            switch (option)
            {
                case FightOptionsEnum.FIGHT_OPTION_ASK_FOR_HELP:
                    AskingForHelp = !AskingForHelp;
                    break;

                case FightOptionsEnum.FIGHT_OPTION_SET_CLOSED:
                    Closed = !Closed;
                    break;

                case FightOptionsEnum.FIGHT_OPTION_SET_TO_PARTY_ONLY:
                    if (Leader is CharacterFighter characterFighter && characterFighter.Character.Party is not null)
                    {
                        foreach (var fighter in _fighters.Values)
                        {
                            if (fighter is CharacterFighter allyCharacter && allyCharacter.Character.Party != characterFighter.Character.Party)
                                RemoveActor(allyCharacter);
                        }

                        RestrictedToParty = !RestrictedToParty;
                    }
                    break;

                case FightOptionsEnum.FIGHT_OPTION_SET_SECRET:
                    // TO DO spectate
                    break;

                default:
                    return;
            }

            Send(FightHandler.SendGameFightOptionStateUpdateMessage, new object[] { this, option });
            Fight.Map.Send(FightHandler.SendGameFightOptionStateUpdateMessage, new object[] { this, option });
        }
        #endregion
    }
}
