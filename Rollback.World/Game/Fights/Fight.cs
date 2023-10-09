using System.Collections.Concurrent;
using System.Drawing;
using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Triggers;
using Rollback.World.Game.Fights.Triggers.Types;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Fights;
using Rollback.World.Network;

namespace Rollback.World.Game.Fights
{
    public abstract class Fight<TChallengers, TDefenders> : ClientCollection<WorldClient, Message>, IFight
        where TChallengers : Team
        where TDefenders : Team
    {
        private readonly TimeLine _timeLine;
        private readonly UniqueIdProvider _contextualIdProvider;
        private readonly UniqueIdProvider _sequenceIdProvider;
        private readonly UniqueIdProvider _triggerIdProvider;
        private readonly SynchronizedCollection<TriggerMark> _triggers;
        private readonly ConcurrentDictionary<int, FightSequence> _sequences;
        private readonly bool _activateBlades;

        protected readonly DateTime _creationDate;
        protected Timer? _timer;

        public short Id { get; }

        public abstract FightTypeEnum Type { get; }

        public abstract bool CanCancelFight { get; }

        public virtual short AgeBonus { get; }

        public MapInstance Map { get; }

        protected readonly TChallengers _challengers;
        public Team Challengers =>
            _challengers;

        protected readonly TDefenders _defenders;
        public Team Defenders =>
            _defenders;

        public IReadOnlyCollection<FightActor> Winners { get; protected set; }

        public IReadOnlyCollection<FightActor> Losers { get; protected set; }

        public FightState State { get; private set; }

        public bool BladesVisible { get; protected set; }

        public bool SpectatorClosed { get; private set; }

        public short RoundNumber =>
            _timeLine.RoundNumber;

        public bool Started =>
            State is FightState.Fighting;

        public int Duration =>
            (int)(DateTime.Now - _creationDate).TotalMilliseconds;

        public bool IsPvP =>
            Type is FightTypeEnum.FIGHT_TYPE_AGRESSION or FightTypeEnum.FIGHT_TYPE_PvT;

        public bool IsSequencing =>
            _sequences.Any(x => !x.Value.Disposed);

        public FightActor? FighterPlaying { get; private set; }

        public ReadyChecker Checker { get; }

        public FightResultListEntry[] Results { get; private set; }

        public FightCommonInformations FightCommonInformations =>
            new(Id,
                (sbyte)Type,
                new[] { Challengers.FightTeamInformations, Defenders.FightTeamInformations },
                new[] { Challengers.BladeCell!.Id, Defenders.BladeCell!.Id },
                new[] { Challengers.FightOptionsInformations, Defenders.FightOptionsInformations });

        public Fight(short id, MapInstance map, TChallengers challengers, TDefenders defenders, bool activateBlades)
        {
            _creationDate = DateTime.Now;
            _timeLine = new(this);
            _contextualIdProvider = new();
            _sequenceIdProvider = new();
            _triggerIdProvider = new();
            _triggers = new();
            _sequences = new();
            _activateBlades = activateBlades;

            _challengers = challengers;
            _challengers.Fight = this;
            _challengers.Side = TeamSide.Challenger;

            _defenders = defenders;
            _defenders.Fight = this;
            _defenders.Side = TeamSide.Defender;

            Id = id;
            Map = map;
            Checker = new(NewTurn, SpotLaggers);

            Winners = Array.Empty<FightActor>();
            Losers = Array.Empty<FightActor>();
            Results = Array.Empty<FightResultListEntry>();
        }

        #region Events
        public event Action<FightActor>? FighterAdded;
        public void OnFighterAdded(FightActor fighter)
        {
            if (fighter is SummonedFighter summon && fighter is not SummonedStaticMonster)
            {
                _timeLine.AddSummon(summon);
                _timeLine.Refresh();
            }

            FighterAdded?.Invoke(fighter);
        }

        public event Action<FightActor>? FighterRemoved;
        public void OnFighterRemoved(FightActor fighter)
        {
            if (State is FightState.Fighting)
            {
                _timeLine.RemoveFighter(fighter);
                _timeLine.Refresh();
            }

            FighterRemoved?.Invoke(fighter);
        }

        public event Action<IFight>? FightEnded;
        public event Action<IFight>? WinnersDeterminated;
        #endregion

        public int GetFreeContextualId() =>
            _contextualIdProvider.Generate();

        protected override IEnumerable<WorldClient> GetClients() =>
            Challengers.GetFighters<CharacterFighter>().Concat(Defenders.GetFighters<CharacterFighter>()).Select(x => x.Character.Client);

        #region Fighters
        public virtual bool CanChangePlacementCell(FightActor fighter, short cellId) =>
            State is FightState.Placement;

        public T? GetFighter<T>(Predicate<T>? p = default)
            where T : FightActor =>
            GetAllFighters(p).FirstOrDefault();

        public T[] GetAllFighters<T>(Predicate<T>? p = default)
            where T : FightActor =>
            Challengers.GetFighters(p).Concat(Defenders.GetFighters(p)).ToArray();

        public List<FightActor> GetAlignedFighters(MapPoint startCell, short range, DirectionsEnum direction)
        {
            var result = new List<FightActor>();
            var actualCell = startCell;

            for (var i = 0; i < range && actualCell is not null && Map.Map.Record.Cells[actualCell.CellId].Walkable &&
                !Map.Map.Record.Cells[actualCell.CellId].NonWalkableDuringFight; i++)
            {
                var fighter = GetFighter<FightActor>(x => x.Alive && x.Cell.Id == actualCell.CellId);

                if (fighter is not null)
                {
                    result.Add(fighter);
                    actualCell = actualCell.GetCellInDirection(direction, 1);
                }
                else
                    actualCell = default;
            }

            return result;
        }
        #endregion

        #region Cells
        public bool IsCellFreeToWalkOn(short cellId) =>
            Cell.CellIdValid(cellId) && Map.Map.Record.Cells[cellId].Walkable && !Map.Map.Record.Cells[cellId].NonWalkableDuringFight && GetAllFighters<FightActor>(x => x.Alive && x.Cell.Id == cellId).Length is 0;

        public Cell? GetFirstFreeCellNear(MapPoint point, bool ignoreActor = false)
        {
            var testedCellIds = new HashSet<int>() { point.CellId };
            var points = new List<MapPoint>(point.GetAdjacentCells());

            while (points.Any())
            {
                var p = points[0];
                points.RemoveAt(0);

                if ((ignoreActor && Map.Map.Record.Cells[p.CellId].Walkable && Map.Map.Record.Cells[p.CellId].NonWalkableDuringFight) || IsCellFreeToWalkOn(p.CellId))
                    return Map.Map.Record.Cells[p.CellId];

                testedCellIds.Add(p.CellId);

                foreach (var adj in p.GetAdjacentCells())
                    if (!testedCellIds.Contains(adj.CellId))
                        points.Add(adj);
            }

            return default;
        }

        /// <summary>
        /// Use without LOS bypassing
        /// </summary>
        /// <returns></returns>
        public bool CanSee(FightActor source, MapPoint target, HashSet<short>? additionalBlockedCells = default)
        {
            // http://playtechs.blogspot.fr/2007/03/raytracing-on-grid.html source : Stump

            List<MapPoint> mapPoints = new();
            var x = source.Cell.Point.X;
            var y = source.Cell.Point.Y;
            var dx = Math.Abs(x - target.X);
            var dy = Math.Abs(y - target.Y);
            var n = 1 + dx + dy;
            var vectorX = (target.X > x) ? 1 : -1;
            var vectorY = (target.Y > y) ? 1 : -1;
            var error = dx - dy;
            dx *= 2;
            dy *= 2;

            for (; n > 0; --n)
            {
                if (MapPoint.IsInMap(x, y))
                    mapPoints.Add(MapPoint.FromCoords(x, y));

                if (error > 0)
                {
                    x += vectorX;
                    error -= dy;
                }
                else if (error == 0)
                {
                    x += vectorX;
                    y += vectorY;
                    n--;
                    error += dx - dy;
                }
                else
                {
                    y += vectorY;
                    error += dx;
                }
            }

            foreach (var point in mapPoints.Where(z => z.CellId != source.Cell.Id && z.CellId != target.CellId))
            {
                var cell = Map.Map.Record.Cells[point.CellId];
                if (!cell.LineOfSight || GetFighter<FightActor>(x => x.Alive && x.GetVisibilityFor(source) is GameActionFightInvisibilityStateEnum.VISIBLE && x.Cell.Id == cell.Id) is not null ||
                    (additionalBlockedCells != null && additionalBlockedCells.Contains(cell.Id)))
                    return false;
            }

            return true;
        }
        #endregion

        #region Timers
        public virtual int GetPlacementTimeLeft()
        {
            var timeLeft = (int)(FightConfig.Instance.PlacementPhaseTime - (DateTime.Now - _creationDate).TotalMilliseconds);

            return timeLeft > 0 ? timeLeft : 0;
        }

        protected virtual int GetTimerTime(FightTimer timerType) =>
            timerType switch
            {
                FightTimer.Placement => FightConfig.Instance.PlacementPhaseTime,
                FightTimer.EndTurn => FightConfig.Instance.TurnTime,
                _ => Timeout.Infinite,
            };

        protected virtual void ExecuteTimerAction(FightTimer timerType)
        {
            switch (timerType)
            {
                case FightTimer.Placement:
                    StartFight();
                    break;

                case FightTimer.EndTurn:
                    FighterPlaying?.EndTurn();
                    break;
            }
        }

        private void OnTimerElapsed(object? sender)
        {
            _timer!.Dispose();

            ExecuteTimerAction((FightTimer)sender!);
        }

        public void StartTimer(FightTimer timerType)
        {
            _timer?.Dispose();

            var period = GetTimerTime(timerType);

            if (period is not Timeout.Infinite)
                _timer = new(new(OnTimerElapsed), timerType, period, Timeout.Infinite);
        }
        #endregion

        #region Blades
        protected virtual void ShowBlades()
        {
            if (!BladesVisible && Challengers.BladeCell is not null && Defenders.BladeCell is not null)
            {
                Map.Send(FightHandler.SendGameRolePlayShowChallengeMessage, new object[] { this });
                BladesVisible = true;
            }
        }

        protected virtual void HideBlades()
        {
            if (BladesVisible)
            {
                Map.Send(FightHandler.SendGameRolePlayRemoveChallengeMessage, new object[] { Id });
                BladesVisible = false;
            }
        }

        private void CheckBladeCellsValidity()
        {
            if (Challengers.BladeCell is null)
                Challengers.BladeCell = Challengers.Leader!.Cell;
            if (Defenders.BladeCell is null)
                Defenders.BladeCell = Challengers.Leader!.Cell;

            if (Challengers.BladeCell.Id == Defenders.BladeCell.Id)
            {
                var cell = Map.GetRandomAdjacentFreeCell(Defenders.BladeCell.Point, true);
                Defenders.BladeCell = cell is null ? Challengers.BladeCell : cell;
            }
        }
        #endregion

        #region Triggers
        private void AddTrigger(TriggerMark trigger)
        {
            _triggers.Add(trigger);

            Send(x => trigger.VisibleFor(x.Account!.Character!.Fighter!), FightHandler.SendGameActionFightMarkCellsMessage, new[] { trigger });
        }

        public void RemoveTrigger(TriggerMark trigger)
        {
            _triggers.Remove(trigger);
            _triggerIdProvider.Free(trigger.Id);

            Send(x => trigger.VisibleFor(x.Account!.Character!.Fighter!), FightHandler.SendGameActionFightUnmarkCellsMessage, new object[] { trigger });
        }

        public void AddGlyph(TriggerType? triggerType, Zone zone, Color color, FightActor caster, Spell spell, short duration) =>
            AddTrigger(new Glyph((short)_triggerIdProvider.Generate(), triggerType, zone, color, caster, spell, duration));

        public void AddTrap(Zone zone, Color color, FightActor caster, Spell spell) =>
            AddTrigger(new Trap((short)_triggerIdProvider.Generate(), zone, color, caster, spell));

        public void DecrementCastedGlyphs(FightActor caster)
        {
            foreach (var glyph in _triggers.OfType<Glyph>().Where(x => x.Caster.Id == caster.Id).ToArray())
            {
                if (glyph.Duration > 0 && --glyph.Duration is 0)
                    RemoveTrigger(glyph);
            }
        }

        public TTrigger[] GetTriggers<TTrigger>(Predicate<TTrigger>? p = default)
            where TTrigger : TriggerMark =>
            (p is null ? _triggers : _triggers.Where(x => x is TTrigger trigger && p(trigger))).Select(x => (TTrigger)x).ToArray();

        public TriggerMark[] GetAvailableTriggersFor(FightActor trigger, TriggerType type) =>
            GetTriggers<TriggerMark>(x => x.TriggerType == type && x.Zone.AffectedCells.ContainsKey(trigger.Cell.Id)).ToArray();

        public void NotifyTriggers(TriggerMark[] triggers, FightActor target)
        {
            var casts = new List<SpellCast>();

            foreach (var trigger in triggers)
                casts.Add(trigger.Trigger(target));

            foreach (var cast in casts)
                cast.ApplyHandlers();
        }
        #endregion

        #region States
        public virtual void StartPlacement()
        {
            if (State is FightState.NotStarted)
            {
                State = FightState.Placement;

                if (_activateBlades)
                {
                    CheckBladeCellsValidity();
                    ShowBlades();
                }

                Map.AddFight(this);

                StartTimer(FightTimer.Placement);
            }
        }

        protected void StartFight()
        {
            if (State is not FightState.Placement)
                Map.AddFight(this);

            HideBlades();

            State = FightState.Fighting;

            Send(FightHandler.SendGameEntitiesDispositionMessage, new object[] { GetAllFighters<FightActor>(x => x.Alive) });
            Send(FightHandler.SendGameFightStartMessage);

            _timeLine.SortFighters();
            _timeLine.Refresh();

            Send(FightHandler.SendGameFightSynchronizeMessage, new object[] { GetAllFighters<FightActor>() });

            NewTurn();
        }
        #endregion

        public virtual void CheckAllStatus()
        {
            if (GetAllFighters<FightActor>().All(x => x.Ready))
            {
                _timer?.Dispose();
                StartFight();
            }
        }

        #region Turn
        private void NewTurn()
        {
            if (FighterPlaying is CharacterFighter characterFighter && _sequences.Count is not 0)
            {
                SpotLaggers(new[] { characterFighter });

                foreach (var sequence in _sequences)
                    sequence.Value.Acknowledge(characterFighter);
            }

            _sequences.Clear();

            FighterPlaying?.ResetUsedPoints();

            FighterPlaying = _timeLine.GetNextFighter();
            FighterPlaying?.StartTurn();
        }

        private void SpotLaggers(CharacterFighter[] laggers)
        {
            if (Checker.Started)
            {
                //<b>En attente du joueur %1...</b>
                //<b>En attente des joueurs %1...</b>
                Send(BasicHandler.SendTextInformationMessage, new object[] { TextInformationTypeEnum.TEXT_INFORMATION_ERROR, (short)(laggers.Length is 1 ? 28 : 29), new string[] { string.Join(", ", laggers.Select(x => x.Character.Name)) } });

                NewTurn();

                var fighterPlayingLagger = laggers.FirstOrDefault(x => x.Id == FighterPlaying!.Id);
                if (fighterPlayingLagger is not null)
                {
                    fighterPlayingLagger.Character.Client.Dispose();
                    // <b>Perte de la connexion : le joueur %1 a été exclu de la partie</b>
                    Send(BasicHandler.SendTextInformationMessage, new object[] { TextInformationTypeEnum.TEXT_INFORMATION_ERROR, (short)30, new string[] { laggers[0].Character.Name } });
                }
            }
        }
        #endregion

        #region Sequences
        public FightSequence? StartSequence(SequenceTypeEnum sequence)
        {
            var seq = default(FightSequence);
            if (!IsSequencing)
            {
                seq = new FightSequence(_sequenceIdProvider.Generate(), sequence, FighterPlaying!);
                _sequences.TryAdd(seq.Id, seq);

                Send(FightHandler.SendSequenceStartMessage, new object[] { seq });
            }

            return seq;
        }

        public void AcknowledgeSequence(CharacterFighter sender, int sequenceId)
        {
            if (_sequences.ContainsKey(sequenceId) && _sequences.TryRemove(sequenceId, out var seq))
                seq.Acknowledge(sender);
        }
        #endregion

        #region End
        protected abstract FightResultListEntry[] GenerateResults();

        public virtual bool CheckFightEnd()
        {
            if (Winners.Count is 0 || Losers.Count is 0)
            {
                if (Challengers.GetFighters<FightActor>(x => x.Alive).Length is 0)
                {
                    Winners = Defenders.GetFighters<FightActor>();
                    Losers = Challengers.GetFighters<FightActor>();
                }
                else if (Defenders.GetFighters<FightActor>(x => x.Alive).Length is 0)
                {
                    Winners = Challengers.GetFighters<FightActor>();
                    Losers = Defenders.GetFighters<FightActor>();
                }
                else
                    return false;

                WinnersDeterminated?.Invoke(this);
            }

            EndFight();

            return true;
        }

        public void EndFight()
        {
            _timer?.Dispose();

            foreach (var sequence in _sequences.Values)
            {
                sequence.Dispose();

                if (FighterPlaying is CharacterFighter characterFighter)
                    AcknowledgeSequence(characterFighter, sequence.Id);
            }

            FighterPlaying?.EndTurn();

            State = FightState.Ended;

            Results = Winners.Count is 0 ? 
                GetAllFighters<FightActor>().Select(x => x.Result.GetResult(FightOutcomeEnum.RESULT_DRAW)).ToArray()
                : GenerateResults();

            foreach (var fighter in GetAllFighters<FightActor>())
                fighter.OnQuitFight();

            Map.RemoveFight(this);
            Map.Map.FightIdProvider.Free(Id);

            FightEnded?.Invoke(this);
        }
        #endregion
    }
}
