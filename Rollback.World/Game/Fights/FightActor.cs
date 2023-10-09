using Rollback.Common.DesignPattern.Collections;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Extensions;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Fights.Buffs;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.History;
using Rollback.World.Game.Fights.Results;
using Rollback.World.Game.Fights.Triggers;
using Rollback.World.Game.Looks;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Game.World.Maps.PathFinding;
using Rollback.World.Handlers.Fights;
using Rollback.World.Handlers.Maps;

namespace Rollback.World.Game.Fights
{
    public abstract class FightActor : ILooter
    {
        private readonly Dictionary<int, SummonedFighter> _summons;
        private readonly UniqueIdProvider _buffIdProvider;

        private SynchronizedCollection<Buff> _buffs;
        private readonly ActorLook _baseLook;

        protected readonly Dictionary<short, Spell> _spells;

        public Team? Team { get; set; }

        public abstract int Id { get; }

        public abstract short Level { get; }

        public ActorLook Look { get; private set; }

        public StatsData Stats { get; protected set; }

        public bool Alive =>
            Stats.Health.Actual > 0;

        private Cell _cell;
        public Cell Cell
        {
            get => _cell;
            set
            {
                _cell = value;

                if (_carriedActor != null)
                    _carriedActor.Cell = value;
            }
        }

        private DirectionsEnum _direction;
        public DirectionsEnum Direction
        {
            get => _direction;
            set
            {
                _direction = value;

                if (_carriedActor is not null)
                    _carriedActor.Direction = value;
            }
        }

        public GameActionFightInvisibilityStateEnum Visibility { get; private set; }


        private FightActor? _carriedActor;
        public FightActor? CarriedActor
        {
            get => _carriedActor;
            private set
            {
                _carriedActor = value;

                if (_carriedActor is not null)
                {
                    _carriedActor.Cell = Cell;
                    _carriedActor.Direction = Direction;
                }
            }
        }

        public IFightResult Result { get; }

        public FightHistory History { get; }

        public abstract FightTeamMemberInformations FightTeamMemberInformations { get; }

        public bool IsLeader =>
            Team is not null && Team.Leader!.Id == Id;

        public virtual bool Ready =>
            true;

        public virtual bool CanMove =>
            Stats.MP.Base >= 0 && Stats.MP.Total > 0;

        public bool Playing =>
            Team?.Fight?.FighterPlaying?.Id == Id;

        public bool CanSummon =>
            Stats[Stat.SummonableCreaturesBoost].Total > _summons.Where(x => x.Value is not SummonedStaticMonster).Count();

        public IdentifiedEntityDispositionInformations IdentifiedEntityDispositionInformations =>
            new(Cell.Id, (sbyte)Direction, Id);

        public virtual GameFightMinimalStats GameFightMinimalStats =>
            new(Stats.Health.Actual,
                Stats.Health.ActualMax,
                Stats.AP.Total,
                Stats.MP.Total,
                0,
                (short)(Stats[Stat.NeutralElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpNeutralElementResistPercent].Total : 0)),
                (short)(Stats[Stat.EarthElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpEarthElementResistPercent].Total : 0)),
                (short)(Stats[Stat.WaterElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpWaterElementResistPercent].Total : 0)),
                (short)(Stats[Stat.AirElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpAirElementResistPercent].Total : 0)),
                (short)(Stats[Stat.FireElementResistPercent].Total + (Team!.Fight.IsPvP ? Stats[Stat.PvpFireElementResistPercent].Total : 0)),
                Stats[Stat.DodgeApLostProbability].Total,
                Stats[Stat.DodgeMpLostProbability].Total,
                (sbyte)Visibility);

        public FightActor(ActorLook look, StatsData stats, Dictionary<short, Spell> spells, Cell cell)
        {
            _spells = spells;
            _summons = new();
            _buffIdProvider = new();
            _buffs = new();
            _baseLook = look;
            _cell = cell;

            Look = look;
            Stats = stats;
            Visibility = GameActionFightInvisibilityStateEnum.VISIBLE;
            Result = IFightResult.CreateResult(this);
            History = new();
        }

        public EntityDispositionInformations EntityDispositionInformations(FightActor fighter) =>
            new(GetVisibilityFor(fighter) is not GameActionFightInvisibilityStateEnum.INVISIBLE ? Cell.Id : (short)-1, (sbyte)Direction);

        public abstract GameFightFighterInformations GameFightFighterInformations(FightActor fighter);

        public bool IsFriendlyWith(FightActor fighter) =>
            fighter.Team!.Side == Team!.Side;

        public FightActor[] GetTacklers()
        {
            var adjacentCells = Cell.Point.GetAdjacentCells().Select(x => x.CellId).ToHashSet();
            return Team!.OpposedTeam.GetFighters<FightActor>(x => x.Alive && x.Visibility is GameActionFightInvisibilityStateEnum.VISIBLE && adjacentCells.Contains(x.Cell.Id));
        }

        public FightActor? GetCarryingActor() =>
            Team!.Fight.GetFighter<FightActor>(x => x.CarriedActor?.Id == Id);

        #region Events
        public event Action? JoinFight;
        public void OnJoinFight()
        {
            Team!.Fight.Send(FightHandler.SendGameFightShowFighterMessage, new object[] { this });

            JoinFight?.Invoke();

            if (Team.Fight.Started)
                Team.Fight.NotifyTriggers(Team.Fight.GetAvailableTriggersFor(this, TriggerType.Move), this);
        }

        public event Action? QuitFight;
        public void OnQuitFight()
        {
            ResetFightBonus();
            QuitFight?.Invoke();
        }

        public event Action? Died;
        #endregion

        #region AP/MP
        public void ResetUsedPoints()
        {
            Stats.AP.Used = 0;
            Stats.MP.Used = 0;
        }

        private void UseAP(short amount)
        {
            if (amount > 0)
            {
                short aP = amount > Stats.AP.Total ? Stats.AP.Total : amount;
                Stats.AP.Used += aP;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterActionPointsUse, this, this, (short)-aP });
            }
        }

        private void UseMP(short amount)
        {
            if (amount > 0)
            {
                short mP = amount > Stats.MP.Total ? Stats.MP.Total : amount;
                Stats.MP.Used += mP;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterMovementPointsUse, this, this, (short)-mP });
            }
        }

        public void LostAP(short amount, FightActor source)
        {
            if (amount > 0)
            {
                short aP = amount > Stats.AP.Total ? Stats.AP.Total : amount;
                Stats.AP.Used += aP;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterActionPointsLost, source, this, (short)-aP });

                Trigger(BuffTriggerType.OnAPLost, source);
            }
        }

        public void LostMP(short amount, FightActor source)
        {
            if (amount > 0)
            {
                short mP = amount > Stats.MP.Total ? Stats.MP.Total : amount;
                Stats.MP.Used += mP;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterMovementPointsLost, source, this, (short)-mP });

                Trigger(BuffTriggerType.OnMPLost, source);
            }
        }

        public void RegainAP(short amount)
        {
            if (amount > 0)
            {
                Stats.AP.Used -= amount;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterActionPointsUse, this, this, amount });
            }
        }

        public void RegainMP(short amount)
        {
            if (amount > 0)
            {
                Stats.MP.Used -= amount;
                Team!.Fight.Send(FightHandler.SendGameActionFightPointsVariationMessage, new object[] { Actions.ActionCharacterMovementPointsUse, this, this, amount });
            }
        }
        #endregion

        protected void ResetFightBonus()
        {
            ResetUsedPoints();

            foreach (var stat in Enum.GetValues(typeof(Stat)))
                Stats[(Stat)stat].Context = 0;
        }

        public void ChangePlacementCell(short cellId)
        {
            if (Team!.CanChangePlacementCell(this, cellId))
            {
                Cell = Team.Fight.Map.Map.Record.Cells[cellId];
                Direction = Team.GetFighterOrientation(this);

                Team.OpposedTeam.UpdateFightersOrientation();
            }
        }

        #region Turns
        public virtual bool StartTurn()
        {
            var result = false;

            if (Team!.Fight.Started && Playing)
            {
                foreach (var characterFighter in Team.Fight.GetAllFighters<CharacterFighter>())
                    characterFighter.Character.RefreshStats();

                using (Team.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_START))
                {
                    Team.Fight.Send(FightHandler.SendGameFightTurnStartMessage, new object[] { Team.Fight });
                    Team.Fight.StartTimer(FightTimer.EndTurn);
                    Team.Fight.Send(FightHandler.SendGameFightSynchronizeMessage, new object[] { Team.Fight.GetAllFighters<FightActor>() });

                    Team.Fight.DecrementCastedGlyphs(this);
                    Trigger(BuffTriggerType.OnTurnBegin, this);

                    Team.Fight.NotifyTriggers(Team.Fight.GetAvailableTriggersFor(this, TriggerType.TurnBegin), this);
                }

                if (_buffs.Any(x => x is SkipTurnBuff))
                    EndTurn();
                else
                    result = true;
            }

            return result;
        }

        public void EndTurn()
        {
            if (Team!.Fight.Started && Playing)
            {
                using (Team.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_TURN_END))
                {
                    Team.Fight.Send(FightHandler.SendGameFightTurnEndMessage, new object[] { Team.Fight });

                    Trigger(BuffTriggerType.OnTurnEnd, this);

                    Team.Fight.NotifyTriggers(Team.Fight.GetAvailableTriggersFor(this, TriggerType.TurnEnd), this);

                    DecrementBuffs();
                }

                Team.Fight.Checker.Start(Team.Fight.GetAllFighters<CharacterFighter>());
            }
        }
        #endregion

        #region Movements
        public void Move(short[] keyMovements)
        {
            if (Alive && CanMove)
            {
                var destinationCellId = Cell.KeyMovementToCellId(keyMovements.Last());

                if (destinationCellId != Cell.Id && Team!.Fight.Map.Map.Record.Cells[destinationCellId].Walkable &&
                    !Team.Fight.Map.Map.Record.Cells[destinationCellId].NonWalkableDuringFight && Playing)
                {
                    var triggers = Array.Empty<TriggerMark>();

                    short usedMP = 0;
                    var stopMove = false;
                    var path = PathFinder.Resolve(keyMovements.Select(x => Cell.KeyMovementToCellId(x)).ToArray(), Team.Fight.Map);
                    var cells = new List<Cell>() { Cell };

                    var i = 1;

                    var carryingActor = GetCarryingActor();
                    if (carryingActor is not null && path.Cells.Length > 1 && Team.Fight.IsCellFreeToWalkOn(path.Cells[1].Id))
                    {
                        carryingActor.Throw(path.Cells[1], true);
                        usedMP++;
                        i++;
                    }

                    using (Team!.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
                    {
                        for (; i < path.Cells.Length && !stopMove; i++)
                        {
                            if (Team.Fight.IsCellFreeToWalkOn(path.Cells[i].Id))
                            {
                                if (Visibility is GameActionFightInvisibilityStateEnum.VISIBLE)
                                {
                                    var tacklers = GetTacklers();
                                    if (tacklers.Length is not 0)
                                    {
                                        var escapeChance = FightFormulas.GetEscapeChance(this, tacklers);
                                        var tackleChance = Random.Shared.NextDouble();

                                        if (tackleChance > escapeChance)
                                        {
                                            if (cells.Count > 1)
                                            {
                                                Team.Fight.Send(MapHandler.SendGameMapMovementMessage, new object[] { Id, new World.Maps.PathFinding.Path(cells.ToArray(), true).KeyMovements });
                                                Trigger(BuffTriggerType.OnMoved, this);
                                                cells.Clear();
                                                cells.Add(Cell);
                                            }

                                            Team.Fight.Send(FightHandler.SendGameActionFightTackledMessage, new object[] { this });

                                            var apBeforeTackle = Stats.AP.Total;

                                            var aPLost = (short)Math.Ceiling(Stats.AP.Total * tackleChance);
                                            LostAP(aPLost, this);

                                            var mPLost = Stats.MP.Total * tackleChance;
                                            if (mPLost > 0)
                                                LostMP((short)(mPLost is 0 && apBeforeTackle is 0 ? 1 : mPLost), this);

                                            Trigger(BuffTriggerType.OnTackled, this);

                                            foreach (var tackler in tacklers)
                                                tackler.Trigger(BuffTriggerType.OnTackle, this);

                                            stopMove = true;
                                        }
                                    }
                                }

                                if (Stats.MP.Total - usedMP > 0)
                                {
                                    Direction = Cell.Point.OrientationTo(path.Cells[i].Point, false);
                                    Cell = path.Cells[i];

                                    usedMP++;
                                    cells.Add(path.Cells[i]);
                                    History.RegisterMovement(Cell);

                                    if ((triggers = Team!.Fight.GetAvailableTriggersFor(this, TriggerType.Move)).Length is not 0 || Stats.MP.Total - usedMP <= 0)
                                        stopMove = true;
                                }
                                else
                                    stopMove = true;
                            }
                            else
                                stopMove = true;
                        }

                        if (cells.Count > 1)
                        {
                            UseMP(usedMP);
                            Team.Fight.Send(x => GetVisibilityFor(x.Account!.Character!.Fighter!) is not GameActionFightInvisibilityStateEnum.INVISIBLE, MapHandler.SendGameMapMovementMessage, new object[] { Id, new World.Maps.PathFinding.Path(cells.ToArray(), true).KeyMovements });
                            Trigger(BuffTriggerType.OnMoved, this);

                            Team.Fight.NotifyTriggers(triggers, this);
                        }
                    }
                }
            }
        }

        public void Push(FightActor source, DirectionsEnum direction, short amount, bool pull)
        {
            if (Alive && amount > 0)
            {
                var triggers = Array.Empty<TriggerMark>();
                var pushInDiagonal = direction.IsDiagonal();
                var distance = pushInDiagonal ? (short)Math.Ceiling(amount / 2d) : amount;

                if (distance is 0)
                    distance = 1;

                var directionsToTest = direction.GetDiagonalDecomposition();

                var startCell = Cell;

                var nextCell = Cell.Point;

                short pushedCells;
                for (pushedCells = 0; pushedCells < distance && nextCell is not null;)
                {
                    MapPoint? cellInDirection;
                    nextCell = Cell.Point.GetCellInDirection(direction, 1);

                    if (nextCell is null || !Team!.Fight.IsCellFreeToWalkOn(nextCell.CellId) ||
                        (pushInDiagonal && directionsToTest.Any(x => (cellInDirection = Cell.Point.GetCellInDirection(x, 1)) is not null && Team!.Fight.IsCellFreeToWalkOn(cellInDirection.CellId))))
                        nextCell = default;

                    if (nextCell is not null)
                    {
                        Cell = Team!.Fight.Map.Map.Record.Cells[nextCell.CellId];

                        if ((triggers = Team.Fight.GetAvailableTriggersFor(this, TriggerType.Move)).Length is not 0)
                        {
                            nextCell = default;
                            pushedCells = distance; // disable damages
                        }

                        pushedCells++;
                    }
                }

                if (pushedCells is not 0)
                {
                    Team!.Fight.Send(FightHandler.SendGameActionFightSlideMessage, new object[] { source, this, startCell.Id });

                    if (CarriedActor is not null)
                        Throw(startCell, true);

                    History.RegisterMovement(Cell);

                    if (!pull)
                        Trigger(BuffTriggerType.OnPushed, source);
                    Trigger(BuffTriggerType.OnMoved, source);
                }

                Team!.Fight.NotifyTriggers(triggers, this);

                if (!pull && pushedCells < distance)
                {
                    var collisionDistance = (short)(distance - pushedCells);
                    byte targetNbr = 0;

                    foreach (var fighter in Team!.Fight.GetAlignedFighters(Cell.Point, distance, direction))
                    {
                        var pushDamageAmount = FightFormulas.CalculatePushBackDamages(source, collisionDistance, targetNbr);

                        if (pushDamageAmount > 0)
                        {
                            fighter.InflictDamage(source, new(pushDamageAmount));

                            fighter.Trigger(BuffTriggerType.OnPushDamaged, source);
                        }

                        if (targetNbr is not 0)
                            fighter.Trigger(BuffTriggerType.OnInderctlyPush, source);

                        targetNbr++;
                    }
                }
            }
        }

        public void Teleport(FightActor source, Cell cell)
        {
            if (Alive)
            {
                var carryingActor = GetCarryingActor();

                if (carryingActor is not null)
                    carryingActor.Throw(cell);
                else if (Team!.Fight.IsCellFreeToWalkOn(cell.Id))
                    Cell = cell;
                else
                {
                    var cellToTp = Team!.Fight.GetFirstFreeCellNear(cell.Point);
                    if (cellToTp is not null)
                    {
                        Cell = cellToTp;
                        History.RegisterMovement(Cell);
                    }
                }

                Team!.Fight.Send(FightHandler.SendGameActionFightTeleportOnSameMapMessage, new object[] { source, this });
            }
        }

        public void ExchangePositions(FightActor with)
        {
            if (with.Id != Id && CarriedActor is null && GetCarryingActor() is null && with.CarriedActor is null)
            {
                (with.Cell, Cell) = (Cell, with.Cell);

                History.RegisterMovement(Cell);
                with.History.RegisterMovement(with.Cell);

                Team!.Fight.Send(FightHandler.SendGameActionFightExchangePositionsMessage, new object[] { this, with });

                Trigger(BuffTriggerType.OnMoved, this);
                with.Trigger(BuffTriggerType.OnMoved, this);
            }
        }

        public void Carry(FightActor target, SpellEffectHandler handler)
        {
            handler.Dispellable = FightDispellable.DispellableByDeath;
            handler.Effect = handler.Effect.Clone();
            handler.Effect.Duration = SpellEffectHandler.InfiniteDuration;
            AddStateBuff(handler, SpellState.Porteur);
            target.AddStateBuff(handler, SpellState.Porte);

            CarriedActor = target;

            Team!.Fight.Send(FightHandler.SendGameActionFightCarryCharacterMessage, new object[] { this, target });

            Died += OnCarryDeath;
            target.Died += OnCarryDeath;
        }

        private void OnCarryDeath() =>
            Throw(Cell, true);

        public void Throw(Cell cell, bool drop = false)
        {
            if (_carriedActor is not null)
            {
                _carriedActor.Cell = cell;
                _carriedActor.History.RegisterMovement(Cell);

                using (Team!.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE))
                {
                    Team.Fight.Send(drop ? FightHandler.SendGameActionFightDropCharacterMessage : FightHandler.SendGameActionFightThrowCharacterMessage, new object[] { this, _carriedActor });

                    foreach (var buff in GetBuffs<StateBuff>(x => x.State is SpellState.Porteur))
                        RemoveBuff(buff, this);

                    foreach (var buff in _carriedActor.GetBuffs<StateBuff>(x => x.State is SpellState.Porte))
                        _carriedActor.RemoveBuff(buff, this);
                }

                _carriedActor = default;
            }
        }
        #endregion

        #region Spells
        public virtual SpellCastResult CanCastSpell(Spell spell, Cell cell)
        {
            var result = SpellCastResult.OK;

            if (!Playing)
                result = SpellCastResult.CannotPlay;
            else if (!_spells.ContainsKey(spell.Id))
                result = SpellCastResult.HasNotSpell;
            else if (!cell.Walkable || cell.NonWalkableDuringFight)
                result = SpellCastResult.UnwalkableCell;
            else if (spell.APCost > Stats.AP.Total)
                result = SpellCastResult.NotEnoughAP;
            else
            {
                var distanceToCastPoint = cell.Point.ManhattanDistanceTo(Cell.Point);
                var spellMaxRange = (spell.RangeCanBeBoosted && Stats[Stat.Range].Total > 0 ? Stats[Stat.Range].Total : 0) + spell.MaxRange;

                var fighterOnCell = Team!.Fight.GetFighter<FightActor>(x => x.Alive && x.Cell.Id == cell.Id);

                if (spell.NeedFreeCell && (fighterOnCell is not null || (spell.Effects.Any(x => EffectManager.IsTriggerMarkEffect(x.Id)) && Team.Fight.GetTriggers<TriggerMark>(x => x.Zone.CenterCell.Id == cell.Id).Length is not 0)))
                    result = SpellCastResult.CellNotFree;
                else if (distanceToCastPoint > spellMaxRange || distanceToCastPoint < spell.MinRange)
                    result = SpellCastResult.NotInZone;
                else if (spell.CastInline && !Cell.Point.IsOnSameLine(cell.Point))
                    result = SpellCastResult.NotInline;
                else
                {
                    var stateBuffs = new HashSet<short>();
                    foreach (var state in _buffs.OfType<StateBuff>())
                    {
                        var stateValue = (short)state.State;
                        stateBuffs.Add(stateValue);
                    }

                    if (spell.StatesRequired.Any(x => !stateBuffs.Contains(x)))
                        result = SpellCastResult.StateRequired;
                    else if (spell.StatesForbidden.Any(x => stateBuffs.Contains(x)) || (_carriedActor is not null && spell.Effects.All(x => x.Id is not EffectId.EffectThrow)))
                        result = SpellCastResult.StateForbidden;
                    else if (spell.CastTestLOS && !Team!.Fight.CanSee(this, cell.Point))
                        result = SpellCastResult.NoLos;
                    else
                    {
                        var lastRoundCast = History.GetLastRoundCast(spell.Id);

                        if (lastRoundCast.HasValue && ((spell.MinCastInterval is not 0 && spell.MinCastInterval + lastRoundCast > Team.Fight.RoundNumber) ||
                            (lastRoundCast == Team.Fight.RoundNumber && ((spell.MaxCastPerTurn is not 0 && History.GetAmountOfCast(Team.Fight.RoundNumber, spell.Id) >= spell.MaxCastPerTurn) || (spell.MaxCastPerTarget is not 0 && fighterOnCell is not null && History.GetAmountOfCastForTarget(Team.Fight.RoundNumber, spell.Id, fighterOnCell.Id) >= spell.MaxCastPerTarget)))))
                            result = SpellCastResult.HistoryError;
                    }
                }
            }

            return result;
        }

        public void CastSpell(Spell spell, Cell cell)
        {
            if (CanCastSpell(spell, cell) is SpellCastResult.OK)
            {
                Direction = Cell.Point.OrientationTo(cell.Point, false);

                var spellCast = new SpellCast(this, spell, cell, FightFormulas.RollCritical(this, spell));

                using (Team!.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
                {
                    Team.Fight.Send(FightHandler.SendGameActionFightSpellCastMessage, new object[] { spellCast });

                    if (Visibility is GameActionFightInvisibilityStateEnum.INVISIBLE)
                        Team.OpposedTeam.Send(FightHandler.SendShowCellMessage, new object[] { this, Cell.Id });

                    UseAP(spell.APCost);

                    spellCast.ApplyHandlers();
                }

                History.RegisterCast(spellCast, Team.Fight.RoundNumber);
            }
        }

        public void CastSpell(short spellId, short cellId)
        {
            if (_spells.ContainsKey(spellId) && Cell.CellIdValid(cellId))
                CastSpell(_spells[spellId], Team!.Fight.Map.Map.Record.Cells[cellId]);
        }
        #endregion

        #region Damages / Heals
        public void InflictDamage(FightActor caster, Damage damage)
        {
            if (Alive)
            {
                if (caster.Id != Id)
                    Direction = Cell.Point.OrientationTo(caster.Cell.Point, false);

                if (damage.ApplyBoost)
                    FightFormulas.CalculateDamage(caster, damage);

                damage.Amount += FightFormulas.CalculateErodedHealth(this, damage.Amount);

                Trigger(BuffTriggerType.BeforeDamaged, caster, damage);

                if (damage.Zone is not null)
                    damage.Amount = FightFormulas.CalculateZoneEfficiency(caster.Cell.Point, Cell.Point, damage.Zone, damage.Amount);

                if (damage.ApplyResistance)
                {
                    FightFormulas.CalculateDamageResistance(this, damage, Team!.Fight.IsPvP);

                    var reflectedDamages = FightFormulas.CalculateDamageReflection(this, damage.Amount);
                    if (reflectedDamages > 0)
                    {
                        Team.Fight.Send(FightHandler.SendGameActionFightReflectDamagesMessage, new object[] { this, caster, reflectedDamages });
                        caster.InflictDamage(this, new(reflectedDamages, EffectSchool.Uknown, applyBoost: false, applyResistance: false));
                    }
                }

                Trigger(BuffTriggerType.OnDamaged, caster, damage);

                if (damage.Amount < 0)
                    damage.Amount = 0;
                else if (damage.Amount > Stats.Health.Actual)
                    damage.Amount = (short)Stats.Health.Actual;

                Stats.Health.Actual -= damage.Amount;

                Team!.Fight.Send(FightHandler.SendGameActionFightLifePointsVariationMessage, new object[] { caster, this, (short)-damage.Amount });

                if (Alive)
                    Trigger(BuffTriggerType.AfterDamaged, this, damage);
                else
                    Kill(caster);
            }
        }

        public void Heal(FightActor caster, short amount, Zone? zone = default, bool applyBoost = true)
        {
            if (Alive)
            {
                if (applyBoost)
                    amount = FightFormulas.CalculateHeal(caster, amount);

                if (zone is not null)
                    amount = FightFormulas.CalculateZoneEfficiency(caster.Cell.Point, Cell.Point, zone, amount);

                Trigger(BuffTriggerType.OnHealed, this, amount);

                if (amount < 0)
                    amount = 0;
                else if (Stats.Health.Actual + amount > Stats.Health.ActualMax)
                    amount = (short)(Stats.Health.ActualMax - Stats.Health.Actual);

                Stats.Health.Actual += amount;
                Team!.Fight.Send(FightHandler.SendGameActionFightLifePointsVariationMessage, new object[] { caster, this, amount });

                Trigger(BuffTriggerType.AfterHealed, this);
            }
        }
        #endregion

        #region Summons
        public void AddSummon(SummonedFighter summon)
        {
            _summons[summon.Id] = summon;
            Team!.AddFighter(summon);
        }

        public void RemoveSummon(SummonedFighter summon)
        {
            if (summon.Alive)
                summon.Kill(this);

            _summons.Remove(summon.Id);
            Team!.RemoveActor(summon);
        }

        public void KillAllSummons()
        {
            foreach (var summon in _summons.Values.ToArray())
                RemoveSummon(summon);
        }
        #endregion

        #region Buffs
        private static SynchronizedCollection<Buff> OrderBuffs(IEnumerable<Buff> buffs) =>
            new(buffs.OrderBy(x => x.Handler.Effect.Record?.Operator is "-").ThenByDescending(x => x.Handler.Effect.Record?.Operator is "+"));

        public TBuff? GetBuff<TBuff>(Predicate<TBuff> p)
            where TBuff : Buff =>
            (TBuff?)_buffs.FirstOrDefault(x => x is TBuff buff && p(buff));

        public TBuff[] GetBuffs<TBuff>(Predicate<TBuff>? p = default)
            where TBuff : Buff =>
            _buffs.Where(x => x is TBuff buff && (p is null || p(buff))).Select(x => (TBuff)x).ToArray();

        private void DecrementBuffs()
        {
            foreach (var buff in _buffs.ToArray())
            {
                if (buff.Duration >= 0 && --buff.Duration <= 0)
                    RemoveBuff(buff, this);
            }
        }

        private void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
            _buffs = OrderBuffs(_buffs);

            Team!.Fight.Send(FightHandler.SendGameActionFightDispellableEffectMessage, new object[] { buff });

            buff.Apply();
        }

        private void RemoveBuff(Buff buff, FightActor source)
        {
            _buffs.Remove(buff);
            buff.Dispell();
            _buffIdProvider.Free(buff.Id);

            if (buff.Duration is not 0)
                Team!.Fight.Send(FightHandler.SendGameActionFightDispellEffectMessage, new object[] { source, this, buff });
        }

        public void DispellBuffs(FightActor source, Predicate<Buff>? p = default)
        {
            foreach (var buff in GetBuffs(p))
                RemoveBuff(buff, source);
        }

        public void AddTriggerBuff(BuffTriggerType type, SpellEffectHandler handler, Action<TriggerBuff, FightActor, BuffTriggerType, object?> applyHandler,
            Action<TriggerBuff>? removeHandler = default, short? customActionId = default) =>
            AddBuff(new TriggerBuff(_buffIdProvider.Generate(), handler, this, type, applyHandler, removeHandler, customActionId));

        public void AddSpellReflectionBuff(SpellEffectHandler handler, short? customActionId = default) =>
            AddBuff(new SpellReflectionBuff(_buffIdProvider.Generate(), handler, this, customActionId));

        public void Trigger(BuffTriggerType type, FightActor trigger, object? token = default)
        {
            foreach (var triggerBuff in GetBuffs<TriggerBuff>(x => x.CanApply(type)))
                triggerBuff.Apply(type, trigger, token);
        }

        public void AddStatBuff(SpellEffectHandler handler, short amount, Stat stat, short? customActionId = null) =>
            AddBuff(new StatBuff(_buffIdProvider.Generate(), handler, this, amount, stat, customActionId));

        public void AddSkipTurnBuff(SpellEffectHandler handler, short? customActionId = null) =>
            AddBuff(new SkipTurnBuff(_buffIdProvider.Generate(), handler, this, customActionId));

        public void AddStateBuff(SpellEffectHandler handler, SpellState state, short? customActionId = null) =>
            AddBuff(new StateBuff(_buffIdProvider.Generate(), handler, this, state, customActionId));

        public void AddInvisibilityBuff(SpellEffectHandler handler, short? customActionId = null) =>
            AddBuff(new InvisibilityBuff(_buffIdProvider.Generate(), handler, this, customActionId));

        public void AddSkinBuff(SpellEffectHandler handler, ActorLook look, short? customActionId = null) =>
            AddBuff(new SkinBuff(_buffIdProvider.Generate(), handler, this, look, customActionId));

        public void AddSpellBuff(SpellEffectHandler handler, short spellId, short amount, short? customActionId = null) =>
            AddBuff(new SpellBuff(_buffIdProvider.Generate(), handler, this, spellId, amount, customActionId));
        #endregion

        #region Visibility
        public GameActionFightInvisibilityStateEnum GetVisibilityFor(FightActor target) =>
            Visibility is GameActionFightInvisibilityStateEnum.INVISIBLE && target.IsFriendlyWith(this) ? GameActionFightInvisibilityStateEnum.DETECTED : Visibility;

        public void SetVisibleState(GameActionFightInvisibilityStateEnum visibility, FightActor source)
        {
            if (Visibility != visibility)
            {
                Visibility = visibility;

                Team!.Fight.Send(FightHandler.SendGameActionFightInvisibilityMessage, new object[] { source, this });

                if (Visibility is GameActionFightInvisibilityStateEnum.VISIBLE)
                    Team.OpposedTeam.Send(FightHandler.SendGameFightShowFighterMessage, new[] { this });
            }
        }
        #endregion

        public void UpdateLook(FightActor source)
        {
            var skinBuff = (SkinBuff?)_buffs.LastOrDefault(x => x is SkinBuff);
            Look = skinBuff is null ? _baseLook : skinBuff.Look;

            Team!.Fight.Send(FightHandler.SendGameActionFightChangeLookMessage, new object[] { source, this });
        }

        public void Kill(FightActor killer)
        {
            using (Team!.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_CHARACTER_DEATH))
            {
                KillAllSummons();

                Trigger(BuffTriggerType.OnDeath, this);

                Stats.Health.Actual = 0;

                Died?.Invoke();

                Team!.Fight.Send(FightHandler.SendGameActionFightDeathMessage, new object[] { killer, this });
            }

            if (!Team!.Fight.CheckFightEnd() && Playing)
                EndTurn();
        }
    }
}
