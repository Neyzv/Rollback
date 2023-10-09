using System.Collections.Concurrent;
using Rollback.Common.Logging;
using Rollback.Common.Network;
using Rollback.Common.Network.Protocol;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Guilds;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Guilds;
using Rollback.World.Network;

namespace Rollback.World.Game.Guilds
{
    public sealed class Guild : ClientCollection<WorldClient, Message>
    {
        private readonly ReaderWriterLockSlim _lockExperience;
        private readonly ReaderWriterLockSlim _lockMaxTaxCollectors;
        private readonly ReaderWriterLockSlim _lockTaxCollectorPods;
        private readonly ReaderWriterLockSlim _lockTaxCollectorProspecting;
        private readonly ReaderWriterLockSlim _lockTaxCollectorWisdom;
        private readonly ReaderWriterLockSlim _lockBoostPoints;

        private readonly GuildRecord _record;
        private readonly ConcurrentDictionary<int, GuildMember> _members;
        private readonly ConcurrentDictionary<int, TaxCollector> _taxCollectors;

        public int Id =>
            _record.Id;

        public string Name
        {
            get => _record.Name;
            private set => _record.Name = value;
        }

        public short Symbol
        {
            get => _record.Symbol;
            private set => _record.Symbol = value;
        }

        public int SymbolColor
        {
            get => _record.SymbolColor;
            private set => _record.SymbolColor = value;
        }

        public short Background
        {
            get => _record.Background;
            private set => _record.Background = value;
        }

        public int BackgroundColor
        {
            get => _record.BackgroundColor;
            private set => _record.BackgroundColor = value;
        }

        public long Experience
        {
            get
            {
                _lockExperience.EnterReadLock();
                try
                {
                    return _record.Experience;
                }
                finally { _lockExperience.ExitReadLock(); }
            }
            private set
            {
                _lockExperience.EnterWriteLock();
                _record.Experience = value;
                _lockExperience.ExitWriteLock();
            }
        }

        public long UpperExperienceLevelFloor { get; private set; }

        public long LowerExperienceLevelFloor { get; private set; }

        public byte Level { get; private set; }

        public sbyte MaxTaxCollector
        {
            get
            {
                _lockMaxTaxCollectors.EnterReadLock();
                try
                {
                    return _record.MaxTaxCollectors;
                }
                finally { _lockMaxTaxCollectors.ExitReadLock(); }
            }
            private set
            {
                _lockMaxTaxCollectors.EnterWriteLock();
                _record.MaxTaxCollectors = value;
                _lockMaxTaxCollectors.ExitWriteLock();
            }
        }

        public short TaxCollectorHealth =>
            (short)(GuildManager.TaxCollectorMaxHealth + (20 * Level));

        public short TaxCollectorDamageBonuses =>
            Level;

        public short TaxCollectorPods
        {
            get
            {
                _lockTaxCollectorPods.EnterReadLock();
                try
                {
                    return _record.TaxCollectorPods;
                }
                finally { _lockTaxCollectorPods.ExitReadLock(); }
            }
            private set
            {
                _lockTaxCollectorPods.EnterWriteLock();
                _record.TaxCollectorPods = value;
                _lockTaxCollectorPods.ExitWriteLock();
            }
        }

        public short TaxCollectorProspecting
        {
            get
            {
                _lockTaxCollectorProspecting.EnterReadLock();
                try
                {
                    return _record.TaxCollectorProspecting;
                }
                finally
                {
                    _lockTaxCollectorProspecting.ExitReadLock();
                }
            }
            private set
            {
                _lockTaxCollectorProspecting.EnterWriteLock();
                _record.TaxCollectorProspecting = value;
                _lockTaxCollectorProspecting.ExitWriteLock();
            }
        }

        public short TaxCollectorWisdom
        {
            get
            {
                _lockTaxCollectorWisdom.EnterReadLock();
                try
                {
                    return _record.TaxCollectorWisdom;
                }
                finally { _lockTaxCollectorWisdom.ExitReadLock(); }
            }
            private set
            {
                _lockTaxCollectorWisdom.EnterWriteLock();
                _record.TaxCollectorWisdom = value;
                _lockTaxCollectorWisdom.ExitWriteLock();
            }
        }

        public short BoostPoints
        {
            get
            {
                _lockBoostPoints.EnterReadLock();
                try
                {
                    return _record.BoostPoints;
                }
                finally
                {
                    _lockBoostPoints.ExitReadLock();
                }
            }
            private set
            {
                _lockBoostPoints.EnterWriteLock();
                _record.BoostPoints = value;
                _lockBoostPoints.ExitWriteLock();
            }
        }

        public IReadOnlyDictionary<short, sbyte> SpellsLevels =>
            _record.SpellsLevels;

        public short MaxMembers =>
            (short)(GuildManager.MinMaxMembersByGuild + Math.Floor(Level / 2d));

        public bool CanAddMembers =>
            _members.Count < MaxMembers;

        public bool CanAddTaxCollector =>
            _taxCollectors.Count < MaxTaxCollector;

        public short HireCost =>
            (short)(GuildManager.BaseHireCost + Level * 100);

        public GuildEmblem Emblem =>
            new(Symbol, SymbolColor, Background, BackgroundColor);

        public Protocol.Types.GuildMember[] MembersInformations =>
            _members.Values.Select(x => x.MemberInformations).ToArray();

        public GuildInformations GuildInformations =>
            new(Name, Emblem);

        public Guild(GuildRecord record, IEnumerable<GuildMember> members)
        {
            _lockExperience = new();
            _lockBoostPoints = new();
            _lockMaxTaxCollectors = new();
            _lockTaxCollectorPods = new();
            _lockTaxCollectorProspecting = new();
            _lockTaxCollectorWisdom = new();

            _record = record;
            _members = new();
            _taxCollectors = new();

            RefreshLevelValues();

            foreach (var member in members)
                if (!_members.TryAdd(member.MemberId, member))
                    Logger.Instance.LogError(msg: $"Can not add member {member.MemberId} into guild {Id}, because he is already added...");
        }

        private void RefreshLevelValues()
        {
            Level = ExperienceManager.Instance.GetGuildLevel(Experience);
            UpperExperienceLevelFloor = ExperienceManager.Instance.GetGuildUpperExperienceLevelFloor(Experience);
            LowerExperienceLevelFloor = ExperienceManager.Instance.GetGuildLowerExperienceLevelFloor(Experience);
        }

        protected override IEnumerable<WorldClient> GetClients() =>
            _members.Values.Where(x => x.Character is not null).Select(x => x.Character!.Client);

        public bool IsMember(int memberId) =>
            _members.ContainsKey(memberId);

        public TaxCollector? GetTaxCollectorById(int taxCollectorId) =>
            _taxCollectors.ContainsKey(taxCollectorId) ? _taxCollectors[taxCollectorId] : default;

        public TaxCollector[] GetTaxCollectors(Predicate<TaxCollector>? p = default) =>
            (p is null ? _taxCollectors.Values : _taxCollectors.Values.Where(x => p(x))).ToArray();

        public bool AddMember(Character character)
        {
            var res = false;

            if (CanAddMembers && GuildManager.Instance.CreateGuildMember(character, this) is { } member &&
                    _members.TryAdd(member.MemberId, member) && character.SetGuild(member))
            {
                GuildHandler.SendGuildJoinedMessage(character.Client);
                GuildHandler.SendGuildInformationsMembersMessage(character.Client);

                character.Refresh();

                if (_members.Count is 1)
                    SetLeader(member.MemberId);
                else
                    member.Refresh();

                res = true;
            }

            return res;
        }

        public void KickMember(int managerId, int memberId)
        {
            var leaving = managerId == memberId;

            if (_members.ContainsKey(memberId) && _members.ContainsKey(managerId) &&
                (leaving || _members[managerId].HasRight(GuildRight.BanMembers)))
            {
                _members.TryRemove(memberId, out _);
                GuildManager.Instance.DeleteGuildMember(_members[memberId]);

                if (_members[memberId].Character is not null)
                    _members[memberId].Character!.QuitGuild();

                if (_members.Count is 0)
                    GuildManager.Instance.DeleteGuild(this);
                else
                {
                    Send(GuildHandler.SendGuildMemberLeavingMessage, new object[] { !leaving, _members[memberId] });

                    if (!leaving && _members[managerId].Character is not null)
                        // Vous avez banni <b>%1</b> de votre guilde.
                        _members[managerId].Character!.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 177, _members[memberId].Name);
                    else if (_members[memberId].Rank is GuildRank.Meneur &&
                        _members.Values.OrderBy(x => x.Rank).ThenByDescending(x => x.GivenXP).FirstOrDefault() is { } newBoss)
                    {
                        SetLeader(newBoss.MemberId);

                        // Ton meneur a quitté la guilde <b>%2</b>, et c\'est <b>%1</b> qui a été désigné pour le remplacer.
                        Send(BasicHandler.SendTextInformationMessage, new object[] {TextInformationTypeEnum.TEXT_INFORMATION_ERROR,
                        (short)199, new[]{ newBoss.Name, Name } });
                    }
                }
            }
        }

        public void SetLeader(int memberId)
        {
            if (_members.ContainsKey(memberId))
            {
                if (_members.Values.FirstOrDefault(x => x.Rank is GuildRank.Meneur) is { } boss)
                {
                    boss.Rank = GuildRank.BrasDroit;
                    boss.ChangeRights(GuildRight.ManageRights);

                    boss.Refresh();
                }

                _members[memberId].Rank = GuildRank.Meneur;
                _members[memberId].ChangeRights(GuildRight.Boss);

                _members[memberId].Refresh();
            }
        }

        public void ManageMemberOptions(int managerId, int memberId, GuildRank rank, sbyte givenXpPercent, params GuildRight[] rights)
        {
            if (_members.ContainsKey(memberId) && _members.ContainsKey(managerId))
            {
                if (rank is GuildRank.Meneur)
                {
                    if (_members[managerId].Rank is GuildRank.Meneur && _members.Values.FirstOrDefault(x => x.Rank is GuildRank.Meneur)?.MemberId != memberId)
                        SetLeader(memberId);
                }
                else
                {
                    if (_members[managerId].HasRight(GuildRight.ManageRanks) && rank <= _members[managerId].Rank)
                        _members[memberId].Rank = rank;

                    if (_members[managerId].HasRight(GuildRight.ManageRights))
                        _members[memberId].ChangeRights(rights);
                }

                if (_members[managerId].HasRight(GuildRight.ManageMembersXP) || (_members[managerId].HasRight(GuildRight.ManageOwnXP) &&
                    _members[memberId].MemberId == _members[managerId].MemberId))
                    _members[memberId].ChangeGivenXPPercent(givenXpPercent);

                _members[memberId].Refresh();
            }
        }

        public void ChangeExperience(long amount)
        {
            if (amount > 0 && (ulong)(Experience + amount) > long.MaxValue)
                Experience = long.MaxValue;
            else if (Experience + amount < 0)
                Experience = 0;
            else
                Experience += amount;

            if (Level < ExperienceManager.Instance.MaxGuildLevel && (Experience < LowerExperienceLevelFloor ||
                (UpperExperienceLevelFloor > 0 && Experience >= UpperExperienceLevelFloor)))
            {
                var lastLevel = Level;

                RefreshLevelValues();

                if (Level > lastLevel)
                    BoostPoints += (short)((Level - lastLevel) * 5);
                // TO DO decrease

                // Votre guilde passe niveau %1
                Send(BasicHandler.SendTextInformationMessage, new object[] { Level });
                Send(GuildHandler.SendGuildLevelUpMessage);
            }
        }

        public void Boost(GuildBoostType boostType)
        {
            if (GuildManager.GetBoostInformations(boostType) is { } boostInfos && BoostPoints >= boostInfos.Item1)
            {
                var boosted = false;

                switch (boostType)
                {
                    case GuildBoostType.Pods:
                        if (TaxCollectorPods < boostInfos.Item3)
                        {
                            TaxCollectorPods += boostInfos.Item2;

                            if (TaxCollectorPods > boostInfos.Item3)
                                TaxCollectorPods = boostInfos.Item3;

                            boosted = true;
                        }
                        break;

                    case GuildBoostType.Prospecting:
                        if (TaxCollectorProspecting < boostInfos.Item3)
                        {
                            TaxCollectorProspecting += boostInfos.Item2;

                            if (TaxCollectorProspecting > boostInfos.Item3)
                                TaxCollectorProspecting = boostInfos.Item3;

                            boosted = true;
                        }
                        break;

                    case GuildBoostType.Wisdom:
                        if (TaxCollectorWisdom < boostInfos.Item3)
                        {
                            TaxCollectorWisdom += boostInfos.Item2;

                            if (TaxCollectorWisdom > boostInfos.Item3)
                                TaxCollectorWisdom = boostInfos.Item3;

                            boosted = true;
                        }
                        break;

                    default:
                        if (MaxTaxCollector < boostInfos.Item3)
                        {
                            MaxTaxCollector += boostInfos.Item2;

                            if (MaxTaxCollector > boostInfos.Item3)
                                MaxTaxCollector = (sbyte)boostInfos.Item3;

                            boosted = true;
                        }
                        break;
                }

                if (boosted)
                {
                    BoostPoints -= boostInfos.Item1;
                    Send(GuildHandler.SendGuildInfosUpgradeMessage);
                }
            }
        }

        public void BoostSpell(short spellId)
        {
            if (SpellsLevels.ContainsKey(spellId) &&
                SpellManager.Instance.GetSpellTemplateById(spellId) is { } template && template.SpellLevels.Length < SpellsLevels[spellId])
            {
                _record.SpellsLevels[spellId]++;
                BoostPoints -= GuildManager.BoostPointToBoostSpell;

                Send(GuildHandler.SendGuildInfosUpgradeMessage);
            }
        }

        public void AddTaxCollector(TaxCollector taxCollector, Cell? cell = default)
        {
            if (_taxCollectors.TryAdd(taxCollector.Id, taxCollector))
            {
                taxCollector.Map.AddTaxCollector(taxCollector, cell);

                Send(GuildHandler.SendTaxCollectorMovementAddMessage, new[] { taxCollector });
            }
        }

        public TaxCollector? RemoveTaxCollector(int taxCollectorId)
        {
            var result = default(TaxCollector);

            if (_taxCollectors.TryRemove(taxCollectorId, out var taxCollector))
            {
                taxCollector.Map.RemoveTaxCollector();
                TaxCollectorManager.Instance.RemoveTaxCollector(taxCollector);

                Send(GuildHandler.SendTaxCollectorMovementRemoveMessage, new[] { taxCollector });

                result = taxCollector;
            }

            return result;
        }

        public void Save()
        {
            _record.SpellsLevelsCSV = string.Join(';', _record.SpellsLevels.ToDictionary(x => x.Key, y => y.Value).Select(x => $"{x.Key},{x.Value}"));
            DatabaseAccessor.Instance.InsertOrUpdate(_record);
        }

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
