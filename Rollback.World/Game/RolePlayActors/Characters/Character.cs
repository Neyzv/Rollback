using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using Rollback.Common.Commands;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Extensions;
using Rollback.World.Game.Accounts.Friends;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Interactions;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Jobs;
using Rollback.World.Game.Looks;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.Quests;
using Rollback.World.Game.RolePlayActors.Characters.Breeds;
using Rollback.World.Game.RolePlayActors.Characters.Parties;
using Rollback.World.Game.RolePlayActors.Monsters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.PathFinding;
using Rollback.World.Game.World.Maps.Triggers;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Characters;
using Rollback.World.Handlers.Compass;
using Rollback.World.Handlers.Context;
using Rollback.World.Handlers.Fights;
using Rollback.World.Handlers.Guilds;
using Rollback.World.Handlers.Interactives;
using Rollback.World.Handlers.Inventory;
using Rollback.World.Handlers.Jobs;
using Rollback.World.Handlers.Maps;
using Rollback.World.Handlers.Mounts;
using Rollback.World.Handlers.Party;
using Rollback.World.Handlers.Quests;
using Rollback.World.Handlers.Social;
using Rollback.World.Handlers.Spells;
using Rollback.World.Network;
using GuildMember = Rollback.World.Game.Guilds.GuildMember;
using Path = Rollback.World.Game.World.Maps.PathFinding.Path;

namespace Rollback.World.Game.RolePlayActors.Characters
{
    public sealed class Character : RolePlayActor, ICommandUser
    {
        private const byte MinLevelForMount = 60;
        
        public const short MaxEnergy = 10_000;
        public const byte ShortcutBarSlotsCount = 100;
        public const byte ShortcutMaxPosition = 63 + ShortcutBarSlotsCount;
        public const byte MaxCellsToWalk = 2;
        private const sbyte MaxMountXpPercent = 90;

        private Character? _followedPartyCharacter;
        private DateTime? _regenStartTime;

        #region Antibot
        private readonly Stopwatch _moveWatch;
        private Path? _path;
        #endregion

        #region Properties
        private readonly CharacterRecord _record;
        private readonly ConcurrentDictionary<short, CharacterSpell> _spells;
        private readonly ConcurrentDictionary<short, CharacterSpell> _spellsToDelete;
        private readonly List<CharacterSpellModification> _spellsModifications;
        private readonly Dictionary<short, Quest> _quests;

        public WorldClient Client { get; }

        public string Name =>
            Client.Account!.Role > GameHierarchyEnum.PLAYER ? $"|{_record.Name}|" : _record.Name;

        public BreedEnum Breed =>
            _record.Breed;

        public bool Sex =>
            _record.Sex;
        
        public ActorLook CharacterLook { get; }

        public byte Level { get; private set; }

        public long Experience
        {
            get => _record.Experience;
            private set => _record.Experience = value;
        }

        public long UpperExperienceLevelFloor { get; private set; }

        public long LowerExperienceLevelFloor { get; private set; }

        public Dictionary<int, Map> KnownZaaps { get; private set; }

        public int? SaveMapId
        {
            get => _record.SaveMapId;
            private set => _record.SaveMapId = value;
        }

        public ushort Honor
        {
            get => _record.Honor;
            private set => _record.Honor = value;
        }

        public ushort Dishonor
        {
            get => _record.Dishonor;
            private set => _record.Dishonor = value;
        }

        public bool PvPEnabled
        {
            get => _record.PvPEnabled;
            private set => _record.PvPEnabled = value;
        }

        public sbyte AlignmentGrade { get; private set; }

        public AlignmentSideEnum AlignmentSide
        {
            get => _record.AlignmentSide;
            private set => _record.AlignmentSide = value;
        }

        public StatsData Stats { get; }

        public short StatsPoint
        {
            get => _record.StatsPoints;
            private set => _record.StatsPoints = value;
        }

        public byte RegenSpeed { get; private set; }

        public bool RegenActive =>
            _regenStartTime.HasValue;

        public short Energy
        {
            get => _record.Energy;
            private set => _record.Energy = value;
        }

        public PlayerLifeStatusEnum LifeState
        {
            get => _record.LifeState;
            private set => _record.LifeState = value;
        }

        public bool IsDead =>
            _record.LifeState is not PlayerLifeStatusEnum.STATUS_ALIVE_AND_KICKING;

        public Inventory Inventory { get; }

        public int Kamas
        {
            get => _record.Kamas;
            private set => _record.Kamas = value;
        }

        public DateTime? LastSalesMessage { get; set; }

        public DateTime? LastSeekMessage { get; set; }

        private IInteraction? _interaction;
        public IInteraction? Interaction
        {
            get => _interaction;
            set
            {
                _interaction = value;

                if (value is not null)
                    Interact?.Invoke(value);
            }
        }

        public InteractiveObject? InteractiveObject { get; set; }

        public bool IsMoving =>
            _path is not null;

        public bool IsBusy =>
            Interaction is not null || InteractiveObject is not null || IsMoving || Fighter is not null;

        public Party? Party { get; set; }

        public Bank Bank { get; }

        public GuildMember? GuildMember { get; private set; }

        public CharacterFighter? Fighter { get; private set; }

        public FriendsBook FriendsBook { get; }

        public Spouse? Spouse { get; private set; }

        private Mount? _equipedMount;
        public Mount? EquipedMount
        {
            get => _record.EquipedMountId.HasValue ? _equipedMount ??= MountManager.Instance.GetMountById(_record.EquipedMountId.Value) : _equipedMount;
            private set
            {
                _record.EquipedMountId = value?.Id;

                _equipedMount = value;
            }
        }

        public sbyte MountXpPercent
        {
            get => _record.MountXpPercent;
            private set => _record.MountXpPercent = value;
        }

        public bool IsRiding
        {
            get => _record.IsRiding;
            private set => _record.IsRiding = value;
        }

        private readonly ConcurrentDictionary<JobIds, Job> _jobs;
        public IReadOnlyDictionary<JobIds, Job> Jobs =>
            _jobs;
        #endregion

        public Character(WorldClient client, CharacterRecord record, MapInstance map, Cell cell, DirectionsEnum direction) :
            base(record.Id, map, cell, direction, ActorLook.Parse(record.BaseEntityLookString))
        {
            Client = client;
            _record = record;

            CharacterLook = Look;

            RefreshLevelValues();

            KnownZaaps = CharacterManager.DeserializeZaaps(_record.BinaryKnownZaaps);

            AlignmentGrade = ExperienceManager.Instance.GetGrade(record.Honor);

            Stats = StatsData.CreateStats(this, record, Level);

            _spells = new();
            _spellsToDelete = new();
            _spellsModifications = new();
            LoadSpells();

            Inventory = new(this);
            Bank = new(this);
            FriendsBook = new(this);

            _jobs = new();
            LoadJobs();

            _quests = new();
            LoadQuests();

            LoadGuild();

            _record.LastSelection = DateTime.Now;

            _moveWatch = new();
        }

        #region Events
        public event Action<Character, MapInstance>? ChangeMap;

        public event Action<IInteraction>? Interact;

        public event Action<Character, IInteraction>? LeaveInteraction;
        public void OnLeaveInteraction(IInteraction interaction) =>
            LeaveInteraction?.Invoke(this, interaction);

        public event Action<IFight>? EnterFight;
        public event Action<IFight>? FightEnded;

        public event Action<Character>? LevelChanged;

        public event Action? Disconnect;
        #endregion

        #region Experience & Level
        private void RefreshLevelValues()
        {
            Level = ExperienceManager.Instance.GetCharacterLevel(Experience);
            UpperExperienceLevelFloor = ExperienceManager.Instance.GetCharacterUpperExperienceLevelFloor(Experience);
            LowerExperienceLevelFloor = ExperienceManager.Instance.GetCharacterLowerExperienceLevelFloor(Experience);
        }

        private void AdjustLevel()
        {
            if (Level < ExperienceManager.Instance.MaxCharacterLevel)
            {
                var lastLevel = Level;

                RefreshLevelValues();

                var levelDifference = (short)(Level - lastLevel);

                if (levelDifference is not 0)
                {
                    Stats.Health.BaseMax += levelDifference * 5;

                    if (levelDifference > 0)
                    {
                        Stats.Health.Actual = Stats.Health.ActualMax;
                        _record.StatsPoints += (short)(5 * levelDifference);
                        _record.SpellsPoints += levelDifference;
                    }
                    else
                    {
                        ResetStats();
                        _record.SpellsPoints = (short)(Level - 1);
                        // TO DO Down grade all spells
                    }

                    Stats.AP.Base = (short)(Level >= 100 ? 7 : 6);

                    var breed = BreedManager.Instance.GetBreedById((int)_record.Breed);
                    if (breed is not null)
                        foreach (var breedSpell in breed.Spells)
                        {
                            var spellTemplate = SpellManager.Instance.GetSpellTemplateById(breedSpell.SpellId);
                            if (spellTemplate is not null && spellTemplate.SpellLevels.Length is not 0)
                            {
                                if (_spells.ContainsKey(spellTemplate.Id))
                                {
                                    while (_spells[spellTemplate.Id].MinPlayerLevel > Level && _spells[spellTemplate.Id].DownGrade()) { }

                                    if (_spells[spellTemplate.Id].MinPlayerLevel > Level)
                                        UnlearnSpell(_spells[spellTemplate.Id].Id, false);
                                }
                                else if (spellTemplate.SpellLevels[0].MinPlayerLevel <= Level)
                                    LearnSpell(spellTemplate.Id, refresh: false, infoMessage: false);
                            }
                        }

                    GuildMember?.Refresh();
                    MapInstance.Send(x => x.Account!.Character!.Id != Id, CharacterHandler.SendCharacterLevelUpInformationMessage, new object[] { this });
                    CharacterHandler.SendCharacterLevelUpMessage(Client, Level);

                    RefreshStats();
                    RefreshSpells();

                    LevelChanged?.Invoke(this);
                }
            }
        }

        public void ChangeExperience(long amount, bool infoMessage = true)
        {
            if (amount > 0 && (ulong)(Experience + amount) > long.MaxValue)
                Experience = long.MaxValue;
            else if (Experience + amount < 0)
                Experience = 0;
            else
                Experience += amount;

            if (Experience < LowerExperienceLevelFloor || (UpperExperienceLevelFloor > 0 && Experience >= UpperExperienceLevelFloor))
                AdjustLevel();

            if (infoMessage)
                // Tu as gagné <b>%1</b> points d\'expérience.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 8, amount);
        }

        public void LevelUp(byte amount)
        {
            if (Level < ExperienceManager.Instance.MaxCharacterLevel)
            {
                Experience = ExperienceManager.Instance.GetCharacterExperienceForLevel((byte)(Level + amount > ExperienceManager.Instance.MaxCharacterLevel ? ExperienceManager.Instance.MaxCharacterLevel : (Level + amount)));
                AdjustLevel();
            }
        }
        #endregion

        #region Stats
        public void BoostStats(StatBoost statBoosted, short boostPoints)
        {
            short amount = 0;

            if (boostPoints > 0 && StatsPoint >= boostPoints)
            {
                var breed = BreedManager.Instance.GetBreedById((int)Breed);
                if (breed is not null)
                {
                    Stat stat = Stat.Strength;
                    var floors = breed.StrengthFloors;

                    switch (statBoosted)
                    {
                        case StatBoost.Vitality:
                            stat = Stat.Vitality;
                            floors = breed.VitalityFloors;
                            break;
                        case StatBoost.Wisdom:
                            stat = Stat.Wisdom;
                            floors = breed.WisdomFloors;
                            break;
                        case StatBoost.Chance:
                            stat = Stat.Chance;
                            floors = breed.ChanceFloors;
                            break;
                        case StatBoost.Agility:
                            stat = Stat.Agility;
                            floors = breed.AgilityFloors;
                            break;
                        case StatBoost.Intelligence:
                            stat = Stat.Intelligence;
                            floors = breed.IntelligenceFloors;
                            break;
                    }

                    amount = BreedManager.AssignStatsPoints(floors!, boostPoints, Stats[stat].Base, out var surplus);
                    Stats[stat].Base += amount;
                    StatsPoint -= (short)(boostPoints - surplus);

                    RefreshStats();
                }
            }

            CharacterHandler.SendStatsUpgradeResultMessage(Client, amount);
        }

        public void ResetStats(bool additional = false)
        {
            Stats[Stat.Vitality].Base = 0;
            Stats[Stat.Wisdom].Base = 0;
            Stats[Stat.Strength].Base = 0;
            Stats[Stat.Intelligence].Base = 0;
            Stats[Stat.Chance].Base = 0;
            Stats[Stat.Agility].Base = 0;

            if (additional)
            {
                Stats[Stat.Vitality].Additional = 0;
                Stats[Stat.Wisdom].Additional = 0;
                Stats[Stat.Strength].Additional = 0;
                Stats[Stat.Intelligence].Additional = 0;
                Stats[Stat.Chance].Additional = 0;
                Stats[Stat.Agility].Additional = 0;
            }

            _record.StatsPoints = (short)(5 * (Level - 1));

            //Tu viens de récupérer %1 points de stats.
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 23, _record.StatsPoints);

            RefreshStats();
        }

        public void AddStatsPoints(short amount, bool refresh = true)
        {
            StatsPoint += amount;

            if (StatsPoint < 0)
                StatsPoint = 0;

            if (refresh)
                RefreshStats();
        }

        public void RegainLife(int amount, bool refresh = true)
        {
            if (amount > 0 && Stats.Health.Actual < Stats.Health.ActualMax)
            {
                var oldHealth = Stats.Health.Actual;
                Stats.Health.Actual += amount;

                if (refresh)
                    RefreshStats();

                //Tu as récupéré <b>%1</b> points de vie.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 1, Stats.Health.Actual - oldHealth);
            }
        }

        public void ChangeEnergy(short amount, bool refresh = true, bool infoMessage = true)
        {
            if (amount is not 0 && Energy < MaxEnergy)
            {
                var energy = (short)(Energy + amount < 0 ? -Energy : Energy + amount > MaxEnergy ? MaxEnergy - Energy : amount);

                Energy += energy;

                if (refresh)
                    RefreshStats();

                if (infoMessage)
                    //"Tu as récupéré <b>%1</b> points d\'énergie." or "Tu as perdu <b>%1</b> points d\'énergie."
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)(amount > 0 ? 7 : 34), energy);
            }
        }

        public void ChangeSpellPoints(short amount, bool refresh = true, bool infoMessage = true)
        {
            amount = _record.SpellsPoints + amount < 0 ? (short)-_record.SpellsPoints : amount;
            _record.SpellsPoints += amount;

            if (refresh)
                RefreshStats();

            if (infoMessage)
                // Tu as gagné <b>%1</b> points de sort.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 16, amount);
        }

        public void AddSpellModification(CharacterSpellModificationType type, short spellId, short amount)
        {
            _spellsModifications.Add(new((sbyte)type, spellId, new(0, amount, 0, amount)));

            RefreshStats();
        }

        public void RemoveSpellModification(Predicate<CharacterSpellModification> p)
        {
            var modification = _spellsModifications.FirstOrDefault(x => p(x));
            if (modification is not null)
                _spellsModifications.Remove(modification);

            RefreshStats();
        }

        public void RefreshStats()
        {
            if (Fighter is null)
            {
                Inventory.CheckEquipedItemsCriterions();
                CharacterHandler.SendSetCharacterRestrictionsMessage(Client);
            }

            Party?.RefreshMember(this);

            CharacterHandler.SendCharacterStatsListMessage(Client);
        }
        #endregion

        #region Look
        public void RefreshLook() =>
            MapInstance?.Send(MapHandler.SendGameContextRefreshEntityLookMessage, new object[] { this });
        #endregion

        #region Spells
        public CharacterSpell? GetSpell(short spellId) =>
            _spells.ContainsKey(spellId) ? _spells[spellId] : default;

        public CharacterSpell[] GetSpells(Predicate<CharacterSpell>? p = default) =>
            (p is null ? _spells.Values : _spells.Values.Where(x => p(x))).ToArray();

        private void LoadSpells()
        {
            foreach (var spellRecord in DatabaseAccessor.Instance.Select<CharacterSpellRecord>(string.Format(CharacterSpellRelator.GetByOwnerId, Id)))
            {
                var template = SpellManager.Instance.GetSpellTemplateById(spellRecord.SpellId);
                if (template is not null && spellRecord.SpellLevel <= template.SpellLevels.Length)
                    _spells.TryAdd(spellRecord.SpellId, new CharacterSpell(spellRecord, this));
                else
                {
                    Logger.Instance.LogWarn($"Force disconnection of client {Client} : Can not load spell {spellRecord.SpellId} with level {spellRecord.SpellLevel}...");
                    Client.Dispose();
                }
            }
        }

        private byte GetNextSpellFreeSlot()
        {
            byte pos = 63;

            for (byte i = 64; i <= ShortcutMaxPosition; i++)
            {
                if (_spells.Values.All(x => x.Position != i))
                {
                    pos = i;
                    break;
                }
            }

            return pos;
        }

        public void RefreshSpells() =>
            InventoryHandler.SendSpellListMessage(Client);

        public void LearnSpell(short id, sbyte level = 1, bool refresh = true, bool infoMessage = true)
        {
            var spellTemplate = SpellManager.Instance.GetSpellTemplateById(id);
            if (spellTemplate is not null)
            {
                if (!_spells.ContainsKey(id) && spellTemplate.SpellLevels.Length >= level && spellTemplate.SpellLevels[level - 1].MinPlayerLevel <= Level)
                {
                    _spellsToDelete.Remove(id, out _);

                    _spells.TryAdd(id, new(spellTemplate, level, this, GetNextSpellFreeSlot()));

                    if (refresh)
                        RefreshSpells();

                    if (infoMessage)
                        // Tu as appris le sort <b>$spell%1</b>.
                        SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 3, id);
                }
                else
                    //Impossible d\'apprendre le sort $spell%1.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 7, id);
            }
        }

        public void UnlearnSpell(short id, bool refresh = true)
        {
            if (_spells.Remove(id, out var spell))
            {
                _spellsToDelete.TryAdd(id, spell);

                if (refresh)
                    RefreshSpells();
            }
        }

        public void BoostSpell(short id, bool refresh = true)
        {
            if (_spells.ContainsKey(id) && _record.SpellsPoints >= _spells[id].Level && _spells[id].Upgrade())
            {
                _record.SpellsPoints -= (short)(_spells[id].Level - 1);

                if (refresh)
                    RefreshStats();

                SpellHandler.SendSpellUpgradeSuccessMessage(Client, _spells[id]);
            }
            else
                SpellHandler.SendSpellUpgradeFailureMessage(Client);
        }

        public void UnBoostSpell(short id, bool refresh = true)
        {
            if (_spells.ContainsKey(id) && _spells[id].DownGrade())
            {
                _record.SpellsPoints += _spells[id].Level;

                if (refresh)
                    RefreshStats();

                SpellHandler.SendSpellUpgradeSuccessMessage(Client, _spells[id]);
            }
        }

        public void MoveSpellShortcut(short spellId, byte pos)
        {
            if (_spells.ContainsKey(spellId))
            {
                if (pos is > 63 and <= ShortcutMaxPosition && _spells[spellId].Position != pos)
                {
                    var oldSpell = _spells.Values.FirstOrDefault(x => x.Position == pos);
                    if (oldSpell is not null)
                    {
                        oldSpell.Position = _spells[spellId].Position;
                        SpellHandler.SendSpellMovementMessage(Client, oldSpell);
                    }

                    _spells[spellId].Position = pos;
                    SpellHandler.SendSpellMovementMessage(Client, _spells[spellId]);
                }
            }
        }
        #endregion

        public void ChangeKamas(int amount, bool infoMessage = true)
        {
            if (Kamas + amount > Inventory.MaxKamasInInventory)
                Kamas = Inventory.MaxKamasInInventory;
            else if (Kamas + amount < 0)
                Kamas = 0;
            else
                Kamas += amount;

            InventoryHandler.SendKamasUpdateMessage(Client);

            if (infoMessage)
                //Tu as gagné %1 kamas.
                //Tu as perdu %1 kamas.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, (short)(amount > 0 ? 45 : 46), Math.Abs(amount));
        }

        #region Movements
        private KeyValuePair<int, short>? GetDefaultSaveMapBySuperAreaId() =>
            MapInstance.Map.SubArea?.Area.SuperArea.Id switch
            {
                3 => new(21757955, 268),
                _ => BreedManager.GetStatueMapCellInfos(Breed)
            };

        public void Teleport(MapInstance instance, short? cellId = null, DirectionsEnum? direction = null, bool forceRefresh = false)
        {
            if (Fighter is null)
            {
                Interaction?.Close();
                CancelMove();

                if (cellId is not null && Cell.CellIdValid(cellId.Value))
                    Cell = instance.Map.Record.Cells[cellId.Value];

                if (direction is not null)
                    Direction = direction.Value;

                if (forceRefresh || instance != MapInstance)
                {
                    MapInstance.RemoveActor(this);
                    MapInstance = instance;
                    instance.AddCharacter(this);

                    ChangeMap?.Invoke(this, MapInstance!);
                }
                else
                    MapInstance.Send(ContextHandler.SendTeleportOnSameMapMessage, new object[] { this });
            }
        }

        public void Teleport(Map map, short? cellId = null, DirectionsEnum? direction = null) =>
            Teleport(map.GetBestInstance(), cellId, direction);

        public void Teleport(int mapId, short? cellId = null, DirectionsEnum? direction = null)
        {
            var map = WorldManager.Instance.GetMapById(mapId);

            if (map is not null)
                Teleport(map, cellId, direction);
            else
            {
                Logger.Instance.LogWarn($"Force disconnection of client {Client} : unknown map {mapId}...");
                Client.Dispose();
            }
        }

        public void TeleportNear(WorldObject worldObject)
        {
            var cell = default(Cell);

            cell = worldObject.MapInstance.Map.Record.Cells[worldObject.Cell.Point.GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_WEST, 1)!.CellId];

            if (!cell.Walkable)
            {
                foreach (var point in worldObject.Cell.Point.GetAdjacentCells(x => worldObject.MapInstance.Map.Record.Cells[x].Walkable))
                {
                    cell = worldObject.MapInstance.Map.Record.Cells[point.CellId];
                    break;
                }
            }

            if (cell?.Walkable == false)
                cell = worldObject.MapInstance.GetFirstFreeCellNearMiddle(true);

            Teleport(worldObject.MapInstance, cell!.Id, cell.Point.OrientationTo(worldObject.Cell.Point).GetOpposedDirection());
        }

        public void TeleportToSpawnPoint(bool fight = false)
        {
            if (SaveMapId.HasValue && WorldManager.Instance.GetMapById(SaveMapId.Value) is { } map &&
                (!fight || MapInstance.Map.SubArea?.Area.SuperArea.Id == map.SubArea?.Area.SuperArea.Id) && map.GetBestInstance() is { } instance)
            {
                if (instance.ContainsZaap && instance.GetInteractive(x => x.Skills.Values.Any(x => x is ZaapSkill)) is { } interactive)
                    TeleportNear(interactive);
                else
                    Teleport(instance, instance.GetFirstFreeCellNearMiddle(true).Id);
            }
            else if (GetDefaultSaveMapBySuperAreaId() is { } tpInfos)
                Teleport(tpInfos.Key, tpInfos.Value);
            else
                Logger.Instance.LogWarn($"Can not find a default save map for breed {Breed}...");
        }

        public void StartMove(IEnumerable<short> keyMovements)
        {
            _moveWatch.Restart();
            _path = PathFinder.Resolve(keyMovements.Select(Cell.KeyMovementToCellId).ToArray(), MapInstance!, allDirections: true);
            MapInstance!.Send(MapHandler.SendGameMapMovementMessage, new object[] { Id, _path.KeyMovements });
        }

        public void StopMove(short? cellId = null)
        {
            _moveWatch.Stop();

            if (_path is not null)
            {
                Move(_path.KeyMovements);

                if (!cellId.HasValue || Cell.CellIdValid(cellId.Value))
                {
                    var pathCount = _path.Cells.Length - 1;

                    var walk = pathCount <= MaxCellsToWalk;

                    if (cellId.HasValue)
                    {
                        Cell = MapInstance!.Map.Record.Cells[cellId.Value];

                        pathCount = 0;
                        for (var i = 1; i < _path.Cells.Length && _path.Cells[i].Id != cellId.Value; i++)
                            pathCount++;

                        if (walk)
                            walk = pathCount <= MaxCellsToWalk;
                    }

                    if (_moveWatch.ElapsedMilliseconds >= pathCount * (walk ? AntiBotConfig.Instance.TimeToTravelWalk : AntiBotConfig.Instance.TimeToTravelRun))
                    {
                        if (cellId is null)
                        {
                            var item = MapInstance!.GetMapItemByCellId(Cell!.Id);
                            if (item is not null)
                            {
                                Inventory.AddItem(item.Item);
                                MapInstance.RemoveItem(Cell.Id);
                            }

                            MapInstance.TriggerCell(this, Cell!.Id, CellTriggerType.Stop);

                            var group = MapInstance.GetActor<MonsterGroup>(x => x.Cell.Id == Cell.Id);
                            if (group is not null && group.IsVisible)
                                FightManager.CreatePvM(MapInstance, this, group)?
                                    .StartPlacement();
                        }
                    }
                    else
                        Client.SafeBotBan();

                    _path = null;
                }
                else
                {
                    Logger.Instance.LogWarn($"Force disconnection of client ${Client} : incorrect cell id to stop on...");
                    Client.Dispose();
                }
            }
            else
            {
                Logger.Instance.LogWarn($"Force disconnection of client ${Client} : can not stop moving until you start...");
                Client.Dispose();
            }
        }

        private void CancelMove()
        {
            _moveWatch.Stop();
            _path = null;
        }

        public void SetSpawnPoint(int mapId)
        {
            SaveMapId = mapId;

            //Position sauvegardée.
            SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 6);
        }
        #endregion

        #region Regen
        public void StartRegen()
        {
            if (RegenActive)
                StopRegen();

            if (Stats.Health.Actual < Stats.Health.ActualMax)
                _regenStartTime = DateTime.Now;

            RegenSpeed = (byte)(10 / GeneralConfiguration.Instance.RegenSpeed);

            CharacterHandler.SendLifePointsRegenBeginMessage(Client, RegenSpeed);
        }

        public void StopRegen()
        {
            if (RegenActive)
            {
                var regainedLife = (int)Math.Floor((DateTime.Now - _regenStartTime!).Value.TotalSeconds / (RegenSpeed / 10));

                if (regainedLife > 0)
                {
                    Stats.Health.Actual += regainedLife;

                    CharacterHandler.SendLifePointsRegenEndMessage(Client, regainedLife);
                    //Tu as récupéré <b>%1</b> points de vie.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 1, regainedLife);
                }

                _regenStartTime = null;
            }
        }
        #endregion

        #region Party
        public void FollowPartyMember(Character target)
        {
            if (Party is not null && Party == target.Party)
            {
                if (_followedPartyCharacter is not null)
                    UnFollowPartyMember();

                _followedPartyCharacter = target;

                PartyHandler.SendPartyFollowStatusUpdateMessage(Client, target.Id);
                CompassHandler.SendCompassUpdatePartyMemberMessage(Client, target);

                // <b>%1</b> suit votre déplacement.
                target.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 52, Name);

                _followedPartyCharacter.ChangeMap += OnFollowedPartyMemberChangeMap;
                _followedPartyCharacter.Disconnect += UnFollowPartyMember;
            }
        }

        public void OnFollowedPartyMemberChangeMap(Character target, MapInstance mapInstance) =>
            CompassHandler.SendCompassUpdatePartyMemberMessage(Client, target);

        public void UnFollowPartyMember()
        {
            if (_followedPartyCharacter is not null)
            {
                // <b>%1</b> ne suit plus votre déplacement.
                _followedPartyCharacter.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 53, Name);

                _followedPartyCharacter.ChangeMap -= OnFollowedPartyMemberChangeMap;
                _followedPartyCharacter.Disconnect -= UnFollowPartyMember;
                _followedPartyCharacter = default;

                PartyHandler.SendPartyFollowStatusUpdateMessage(Client, 0);
            }
        }
        #endregion

        #region Jobs
        private void LoadJobs()
        {
            foreach (var jobRecord in DatabaseAccessor.Instance.Select<CharacterJobRecord>(string.Format(CharacterJobRelator.GetJobsByOwnerId, Id)))
                if (_jobs.Count < JobManager.MaxJobCount)
                    _jobs.TryAdd(jobRecord.JobId, new(this, jobRecord));
                else
                    DatabaseAccessor.Instance.Delete(jobRecord);
        }

        public bool HaveJob(JobIds job) =>
            job <= JobManager.BaseJobId || Jobs.ContainsKey(job);

        public void RefreshJobs()
        {
            JobHandler.SendJobDescriptionMessage(Client);
            JobHandler.SendJobExperienceMultiUpdateMessage(Client);
            JobHandler.SendJobCrafterDirectorySettingsMessage(Client);

            foreach (var interactive in MapInstance.GetInteractives())
                InteractiveHandler.SendInteractiveElementUpdatedMessage(Client, interactive);
        }

        public bool LearnJob(JobIds jobId)
        {
            var result = false;

            if (!_jobs.ContainsKey(jobId) && JobManager.Instance.GetRecordById(jobId) is { } jobTemplate &&
                _jobs.Count < JobManager.MaxJobCount)
            {
                var job = new Job(this, new()
                {
                    JobId = jobId,
                    OwnerId = Id
                });

                _jobs.TryAdd(jobId, job);

                RefreshJobs();

                // Tu as appris le métier <b>$job%1</b>.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 2, (int)jobId);
            }

            return result;
        }
        #endregion

        #region Guild
        private void LoadGuild()
        {
            if (GuildManager.Instance.GetGuildMemberById(Id) is { } member)
                if (!(GuildManager.Instance.GetGuildById(member.GuildId) is { } guild) ||
                    !SetGuild(member))
                {
                    Logger.Instance.LogWarn($"Force disconnection of client {Client} : can not assign a guild...");
                    Client.Dispose();
                }
        }

        public bool SetGuild(GuildMember guildMember)
        {
            var res = false;

            if (GuildMember is null && guildMember.Guild.IsMember(Id) && guildMember.AssignCharacter(this))
            {
                GuildMember = guildMember;

                res = true;
            }

            return res;
        }

        public void QuitGuild()
        {
            if (GuildMember is not null)
            {
                GuildMember.UnassignCharacter();
                GuildMember = default;

                Refresh();

                GuildHandler.SendGuildLeftMessage(Client);
                // Vous avez quitté la guilde.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 176);
            }
        }
        #endregion

        #region Quests
        private void LoadQuests()
        {
            foreach (var characterQuestRecord in DatabaseAccessor.Instance.Select<CharacterQuestRecord>(
                string.Format(CharacterQuestRelator.GetCharacterQuestsByCharacterId, Id)))
            {
                if (!_quests.TryAdd(characterQuestRecord.QuestId, new(this, characterQuestRecord)))
                {
                    Logger.Instance.LogWarn(msg: $"Force disconnection of client {Client} : error while loading quests...");
                    Client.Dispose();
                }
            }
        }

        public Quest[] GetQuests(Predicate<Quest>? p = default) =>
            (p is null ? _quests.Values : _quests.Values.Where(x => p(x))).ToArray();

        public Quest? GetQuest(short id) =>
            _quests.ContainsKey(id) ? _quests[id] : default;

        public Quest? GetQuest(Predicate<Quest> p) =>
            _quests.Values.FirstOrDefault(x => p(x));

        public bool CanStartQuest(short questId) =>
            !_quests.TryGetValue(questId, out var quest) || (quest.IsFinished && quest.IsRepeatable);

        public void StartQuest(short questId)
        {
            var questTemplate = QuestManager.Instance.GetQuestRecordById(questId);

            if (questTemplate is not null)
            {
                if (questTemplate.Steps.Length is not 0)
                {
                    if (CanStartQuest(questId))
                    {
                        _quests[questId] = new(this, new()
                        {
                            QuestId = questId,
                            CharacterId = Id,
                            CurrentStepId = questTemplate.Steps.First().Id,
                        });

                        // Nouvelle quête : <b>$quest%1</b>
                        SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 54, questId);

                        QuestHandler.SendMapNpcsQuestStatusUpdateMessage(Client);

                        foreach (var interactive in MapInstance.GetInteractives())
                            InteractiveHandler.SendInteractiveElementUpdatedMessage(Client, interactive);
                    }
                    else
                        SendServerMessage($"You actually have this quest...", Color.OrangeRed);
                }
                else
                    SendServerMessage($"Can not start quest {questId}, this quest doesn't have any steps...", Color.OrangeRed);
            }
            else
                SendServerMessage($"Can not start quest {questId}, this quest doesn't exist...", Color.OrangeRed);
        }

        public void CompleteQuestObjective(short questId, short objectiveId)
        {
            if (GetQuest(questId) is { } quest &&
                quest.GetObjective(x => x.Id == objectiveId) is { } objective)
                objective.Complete();
        }
        #endregion

        #region Fights
        public FighterRefusedReasonEnum CanRequestFight(Character target, bool friendly) // TO DO
        {
            var result = FighterRefusedReasonEnum.FIGHTER_ACCEPTED;

            if (target.IsBusy)
                result = FighterRefusedReasonEnum.OPPONENT_OCCUPIED;
            else if (IsBusy)
                result = FighterRefusedReasonEnum.IM_OCCUPIED;
            else if (target.Id == Id)
                result = FighterRefusedReasonEnum.FIGHT_MYSELF;
            else if (MapInstance?.Map.Record.Id != target.MapInstance?.Map.Record.Id || !MapInstance!.Map.AllowFights)
                result = FighterRefusedReasonEnum.WRONG_MAP;
            else if (!friendly && AlignmentSide == target.AlignmentSide)
                result = FighterRefusedReasonEnum.WRONG_ALIGNMENT;

            return result;
        }

        public FighterRefusedReasonEnum CanRequestFight(TaxCollectorNpc taxCollector)
        {
            var result = FighterRefusedReasonEnum.FIGHTER_ACCEPTED;

            if (GuildMember?.GuildId == taxCollector.TaxCollector.Guild.Id)
                result = FighterRefusedReasonEnum.WRONG_GUILD;
            else if (IsBusy)
                result = FighterRefusedReasonEnum.IM_OCCUPIED;
            else if (taxCollector.TaxCollector.IsBusy)
                result = FighterRefusedReasonEnum.OPPONENT_OCCUPIED;
            else if (taxCollector.MapInstance.Map.Record.Id != MapInstance.Map.Record.Id)
                result = FighterRefusedReasonEnum.WRONG_MAP;

            return result;
        }

        public CharacterFighter? JoinTeam(Team team)
        {
            if (Fighter is null && !team.Full)
            {
                MapInstance.RemoveActor(this);
                StopRegen();

                ContextHandler.SendGameContextDestroyMessage(Client);
                ContextHandler.SendGameContextCreateMessage(Client, GameContextEnum.FIGHT);

                Fighter = new CharacterFighter(this);
                if (team.AddFighter(Fighter))
                {
                    //TO DO Group
                    EnterFight?.Invoke(team.Fight);
                }
                else
                    Client.Dispose();
            }

            return Fighter;
        }

        public void QuitFight()
        {
            if (Fighter is not null)
            {
                FightHandler.SendGameFightEndMessage(Client, Fighter.Team!.Fight!, Fighter.Team!.Fight.Results);

                RefreshStats();
                StartRegen();

                FightEnded?.Invoke(Fighter.Team!.Fight!);

                Fighter = null;
            }
        }
        #endregion

        #region Spouse
        public void LoadSpouse()
        {
            if (_record.SpouseId.HasValue)
            {
                if (CharacterManager.GetCharacterRecordById(_record.SpouseId.Value) is { } spouse && SetSpouse(spouse))
                {
                    if (WorldServer.Instance.GetClient(x => x.Account?.Character?.Id == spouse.Id)?.Account!.Character is { } character)
                    {
                        if (character.Spouse is not null)
                        {
                            character.Spouse.SetOnline(this);
                            Spouse!.SetOnline(character);
                        }
                        else
                        {
                            _record.SpouseId = default;
                            Spouse = default;
                        }
                    }
                }
                else
                    _record.SpouseId = default;
            }
        }

        public bool SetSpouse(CharacterRecord record)
        {
            var result = false;

            if (Spouse is null)
            {
                Spouse = new(this, record);
                result = true;
            }

            return result;
        }

        public void UnsetSpouse()
        {
            if (Spouse is not null)
            {
                Spouse = default;
                _record.SpouseId = default;

                SocialHandler.SendFriendsListMessage(Client);

                // Etre marié ou ne pas l\'être... Tu sembles ne pas avoir fait le bon choix.
                SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 35);
            }
        }

        public void Marry(Character character)
        {
            if (character.Spouse is null && Spouse is null && CharacterManager.GetCharacterRecordById(character.Id) is { } spouse &&
                SetSpouse(spouse) && character.SetSpouse(_record))
            {
                character.Spouse!.SetOnline(this);
                Spouse!.SetOnline(character);
            }
        }

        public void Divorce()
        {
            if (Spouse is not null)
            {
                Spouse.Character?.UnsetSpouse();
                UnsetSpouse();
            }
        }
        #endregion

        #region Mounts        
        public bool TryEquipMount(Mount mount)
        {
            var result = false;
            
            if (mount.AccountId == Client.Account!.Id)
            {
                if (Level >= MinLevelForMount)
                {
                    Dismount();
                    EquipedMount = mount;
                    EquipedMount.SetOwner(this);

                    MountHandler.SendMountSetMessage(Client);
                    MountHandler.SendMountXpRatioMessage(Client);

                    result = true;
                }
                else
                    // Tu dois atteindre le <b>niveau %1</b> pour utiliser cette monture.
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 227,
                        MinLevelForMount);
            }

            return result;
        }

        public void UnEquipMount()
        {
            if (EquipedMount is not null)
            {
                Dismount();

                EquipedMount = null;
                MountHandler.SendMountUnSetMessage(Client);
            }
        }

        public void RideMount()
        {
            if (EquipedMount is not null && !IsRiding)
            {
                IsRiding = true;

                if (Inventory.GetEquipedItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS) is { } pet)
                {
                    Inventory.MoveItem(pet, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                    
                    // Votre familier ne peut vous suivre tant que vous êtes sur votre monture...
                    SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 88);
                }
                
                MountHandler.SendMountRidingMessage(Client);

                CharacterLook.Bones = 2;
                Look = EquipedMount.Look.Clone();
                Look.SetSubLook(new SubActorLook(0, SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_MOUNT_DRIVER, CharacterLook));
                
                RefreshLook();
                
                EquipedMount.HandleMountEffects(true);
            }
        }

        public void Dismount()
        {
            if (EquipedMount is not null && IsRiding)
            {
                IsRiding = false;
                
                MountHandler.SendMountRidingMessage(Client);

                CharacterLook.Bones = 1;
                Look = CharacterLook;
                RefreshLook();
                
                EquipedMount.HandleMountEffects(false);
            }
        }

        public void ChangeMountXpPercent(sbyte xpPercent)
        {
            MountXpPercent = (sbyte)(xpPercent > MaxMountXpPercent ? MaxMountXpPercent : xpPercent < 0 ? 0 : xpPercent);
            MountHandler.SendMountXpRatioMessage(Client);
        }
        #endregion

        #region Message
        public void SendInformationMessage(TextInformationTypeEnum msgType, short msgId, params object[] parameters) =>
            BasicHandler.SendTextInformationMessage(Client, msgType, msgId, parameters.Select(x => x.ToString()!).ToArray());

        public void SendServerMessage(string message) =>
            BasicHandler.SendTextInformationMessage(Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 0, message);

        public void SendServerMessage(string message, string color) =>
            SendServerMessage($"<font color=\"{(color[0] is '#' ? color : $"#{color}")}\">{message}</font>");

        public void SendServerMessage(string message, Color color) =>
            SendServerMessage($"<font color=\"#{color.ToArgb():X}\">{message}</font>");
        #endregion

        #region Commands
        public byte Role =>
            (byte)Client.Account!.Role;

        public string ToBold(string text) =>
            $"<b>{text}</b>";

        public void Reply(string message) =>
            SendServerMessage(message);

        public void ReplyError(string message) =>
            SendServerMessage(message, Color.OrangeRed);
        #endregion

        #region PacketInformations
        public ActorExtendedAlignmentInformations ActorExtendedAlignmentInformations =>
            new((sbyte)_record.AlignmentSide,
                0,
                (sbyte)(_record.PvPEnabled ? AlignmentGrade : 0),
                0,
                _record.Honor,
                _record.Dishonor,
                _record.PvPEnabled);

        public CharacterCharacteristicsInformations CharacterCharacteristicsInformations =>
            new(Experience,
                LowerExperienceLevelFloor,
                UpperExperienceLevelFloor,
                Kamas,
                _record.StatsPoints,
                _record.SpellsPoints,
                ActorExtendedAlignmentInformations,
                Stats.Health.Actual,
                Stats.Health.ActualMax,
                Energy,
                MaxEnergy,
                Stats.AP.Total,
                Stats.MP.Total,
                Stats[Stat.Initiative].CharacterBaseCharacteristic,
                Stats[Stat.Prospecting].CharacterBaseCharacteristic,
                Stats[Stat.ActionPoints].CharacterBaseCharacteristic,
                Stats[Stat.MovementPoints].CharacterBaseCharacteristic,
                Stats[Stat.Strength].CharacterBaseCharacteristic,
                Stats[Stat.Vitality].CharacterBaseCharacteristic,
                Stats[Stat.Wisdom].CharacterBaseCharacteristic,
                Stats[Stat.Chance].CharacterBaseCharacteristic,
                Stats[Stat.Agility].CharacterBaseCharacteristic,
                Stats[Stat.Intelligence].CharacterBaseCharacteristic,
                Stats[Stat.Range].CharacterBaseCharacteristic,
                Stats[Stat.SummonableCreaturesBoost].CharacterBaseCharacteristic,
                Stats[Stat.Reflect].CharacterBaseCharacteristic,
                Stats[Stat.CriticalHit].CharacterBaseCharacteristic,
                0, // TO DO
                Stats[Stat.CriticalMiss].CharacterBaseCharacteristic,
                Stats[Stat.HealBonus].CharacterBaseCharacteristic,
                Stats[Stat.AllDamagesBonus].CharacterBaseCharacteristic,
                Stats[Stat.WeaponDamagesBonusPercent].CharacterBaseCharacteristic,
                Stats[Stat.DamagesBonusPercent].CharacterBaseCharacteristic,
                Stats[Stat.TrapBonus].CharacterBaseCharacteristic,
                Stats[Stat.TrapBonusPercent].CharacterBaseCharacteristic,
                Stats[Stat.PermanentDamagePercent].CharacterBaseCharacteristic,
                Stats[Stat.DodgeApLostProbability].CharacterBaseCharacteristic,
                Stats[Stat.DodgeMpLostProbability].CharacterBaseCharacteristic,
                Stats[Stat.NeutralElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.EarthElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.WaterElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.AirElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.FireElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.NeutralElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.EarthElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.WaterElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.AirElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.FireElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.PvpNeutralElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.PvpEarthElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.PvpWaterElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.PvpAirElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.PvpFireElementResistPercent].CharacterBaseCharacteristic,
                Stats[Stat.PvpNeutralElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.PvpEarthElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.PvpWaterElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.PvpAirElementReduction].CharacterBaseCharacteristic,
                Stats[Stat.PvpFireElementReduction].CharacterBaseCharacteristic,
                _spellsModifications.ToArray());

        public ActorRestrictionsInformations ActorRestrictionsInformations => // TO DO
            new(IsDead, // cantBeAgressed
                IsDead, // cantBeChallenged
                IsDead, // cantTrade
                IsDead, // cantBeAttackedByMutant
                false, // cantRun
                IsDead || Inventory.IsFull, // forceSlowWalk
                false, // cantMinimize
                LifeState is PlayerLifeStatusEnum.STATUS_TOMBSTONE, // cantMove
                IsDead, // cantAggress
                IsDead, // cantChallenge
                IsDead, // cantExchange
                IsDead, // cantAttack
                false, // cantChat
                IsDead, // cantBeMerchant
                IsDead, // cantUseObject
                IsDead, // cantUseTaxCollector
                IsDead, // cantUseInteractives
                IsDead, // cantSpeakToNPC
                false, // cantChangeZone
                IsDead, // cantAttackMonster
                false); // cantWalk8Directions

        public HumanInformations HumanInformations => // TO DO
            GuildMember is null ? new HumanInformations(Array.Empty<EntityLook>(),
                0,
                0,
                ActorRestrictionsInformations,
                0)
            : new HumanWithGuildInformations(Array.Empty<EntityLook>(),
                0,
                0,
                ActorRestrictionsInformations,
                0,
                GuildMember.Guild.GuildInformations);

        public ActorAlignmentInformations ActorAlignmentInformations => // TO DO
            new((sbyte)_record.AlignmentSide,
                0,
                (sbyte)(_record.PvPEnabled ? AlignmentGrade : 0),
                0);

        public override GameRolePlayActorInformations GameRolePlayActorInformations(Character character) =>
            new GameRolePlayCharacterInformations(
                Id,
                Look.GetEntityLook(),
                EntityDispositionInformations,
                Name,
                HumanInformations,
                ActorAlignmentInformations);

        public CharacterBaseInformations CharacterBaseInformations =>
            new(Id,
                Name,
                Level,
                Look.GetEntityLook(),
                (sbyte)Breed,
                _record.Sex);

        public PartyMemberInformations PartyMemberInformations =>
            new(Id,
                Name,
                Level,
                Look.GetEntityLook(),
                Stats.Health.Actual,
                Stats.Health.ActualMax,
                Stats[Stat.Prospecting].Total,
                RegenSpeed,
                Stats[Stat.Initiative].Total,
                PvPEnabled,
                (sbyte)AlignmentSide);

        public CharacterMinimalPlusLookInformations CharacterMinimalPlusLookInformations =>
            new(Id, Name, Level, Look.GetEntityLook());
        #endregion

        public void LeaveGame()
        {
            Disconnect?.Invoke();
            UnFollowPartyMember();
            Party?.Leave(this);
            GuildMember?.UnassignCharacter();

            foreach (var client in WorldServer.Instance.GetClients(x => x.Account?.Character is not null))
                client.Account!.Character!.FriendsBook.NotifyDisconnection(this);

            if (Spouse is not null && Spouse.Character is not null)
            {
                Spouse.Character.Spouse!.SetOffline();
                Spouse.SetOffline();
            }

            Save();

            MapInstance?.RemoveActor(this);
        }

        private void Save()
        {
            _record.Health = Stats.Health.Actual;
            _record.AP = (byte)Stats.AP.Base;
            _record.MP = (byte)Stats.MP.Base;
            _record.Vitality = Stats[Stat.Vitality].Base;
            _record.Wisdom = Stats[Stat.Wisdom].Base;
            _record.Strength = Stats[Stat.Strength].Base;
            _record.Intelligence = Stats[Stat.Intelligence].Base;
            _record.Chance = Stats[Stat.Chance].Base;
            _record.Agility = Stats[Stat.Agility].Base;
            _record.PermanentVitality = Stats[Stat.Vitality].Additional;
            _record.PermanentWisdom = Stats[Stat.Wisdom].Additional;
            _record.PermanentStrength = Stats[Stat.Strength].Additional;
            _record.PermanentIntelligence = Stats[Stat.Intelligence].Additional;
            _record.PermanentChance = Stats[Stat.Chance].Additional;
            _record.PermanentAgility = Stats[Stat.Agility].Additional;

            _record.MapId = MapInstance.Map.Record.Id;
            _record.CellId = Cell.Id;
            _record.Direction = Direction;

            _record.BinaryKnownZaaps = CharacterManager.SerializeZaaps(KnownZaaps);

            _record.EntityLookString = Look.ToString();

            if (Spouse is not null)
                _record.SpouseId = Spouse.Id;

            foreach (var spell in _spellsToDelete.Values)
                spell.Delete();
            _spellsToDelete.Clear();

            foreach (var spell in _spells.Values)
                spell.Save();

            Inventory.Save();
            Bank.Save();
            FriendsBook.Save();

            foreach (var job in Jobs.Values)
                job.Save();

            foreach (var quest in _quests.Values)
                quest.Save();

            DatabaseAccessor.Instance.Update(_record);
            Logger.Instance.LogInfo($"Character {Name} saved successfully.");
        }
    }
}
