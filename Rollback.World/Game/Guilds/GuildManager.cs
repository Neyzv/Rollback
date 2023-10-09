using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Extensions;
using Rollback.Common.Initialization;
using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Guilds;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Guilds
{
    public sealed class GuildManager : Singleton<GuildManager>
    {
        public const short GuildalogemmeId = 1575;
        public const byte MinMaxMembersByGuild = 30;
        public const sbyte MaxGivenXpPercent = 90;
        public const int MaxGuildXP = 300_000;
        public const short BaseHireCost = 1000;

        public const sbyte BaseMaxTaxCollector = 1;
        public const short TaxCollectorMaxHealth = 3_000;
        public const short BaseTaxCollectorPods = 1_000;
        public const short BaseTaxCollectorProspecting = 100;
        public const short TaxCollectorsBones = 714;

        public const byte BoostPointToBoostSpell = 5;

        private static readonly Regex _nameRegex;

        private readonly ConcurrentDictionary<int, Guild> _guilds;
        private readonly ConcurrentQueue<Guild> _guildsToDelete;
        private readonly ConcurrentDictionary<int, GuildMember> _guildMembers;
        private readonly ConcurrentQueue<GuildMember> _guildMembersToDelete;
        private readonly HashSet<short> _availablesBackgrounds;
        private readonly HashSet<short> _availablesShapes;
        private readonly UniqueIdProvider _idProvider;

        static GuildManager() =>
            _nameRegex = new("^(?=[A-Z][a-z]+(?:[- ][A-Za-z][a-z]+){0,2}$)(?=.{3,31}$)", RegexOptions.Compiled);

        public GuildManager()
        {
            _guilds = new();
            _guildsToDelete = new();
            _guildMembers = new();
            _guildMembersToDelete = new();
            _availablesBackgrounds = new();
            _availablesShapes = new();
            _idProvider = new();
        }

        private static readonly IReadOnlyCollection<short> _taxCollectorsSpells = new short[]
        {
            458,
            457,
            456,
            455,
            462,
            460,
            459,
            451,
            453,
            454,
            452,
            461
        };

        private static readonly IReadOnlyDictionary<GuildBoostType, (byte, sbyte, short)> _guildBoostInfos = new Dictionary<GuildBoostType, (byte, sbyte, short)>()
        {
            [GuildBoostType.Pods] = (1, 20, 5000),
            [GuildBoostType.Wisdom] = (1, 1, 400),
            [GuildBoostType.Prospecting] = (1, 1, 500),
            [GuildBoostType.TaxCollectors] = (10, 1, 50)
        };

        public static bool IsNameCorrect(string name) =>
            _nameRegex.IsMatch(name);

        [Initializable(InitializationPriority.DependantDatasManager, "Guilds")]
        public void Initialize()
        {
            Logger.Instance.Log("\tLoading emblems...");
            foreach (var background in DatabaseAccessor.Instance.Select("select * from guilds_backgrounds"))
                if (background.TryGetValue("Id", out var backgroundO) && backgroundO.ChangeType<short>() is { } backgroundId)
                    _availablesBackgrounds.Add(backgroundId);

            foreach (var foreground in DatabaseAccessor.Instance.Select("select * from guilds_symbols"))
                if (foreground.TryGetValue("Id", out var foregroundO) && foregroundO.ChangeType<short>() is { } foregroundId)
                    _availablesShapes.Add(foregroundId);

            foreach (var member in DatabaseAccessor.Instance.Select<GuildMemberRecord>(GuildMemberRelator.GetMembers))
            {
                if (member.MemberRecord is not null)
                {
                    if (!_guildMembers.TryAdd(member.MemberId, new(member)))
                        Logger.Instance.LogError(msg: $"Found multiple {nameof(GuildMemberRecord)} for character {member.MemberId}...");
                }
                else
                {
                    Logger.Instance.LogInfo($"Deletion of guild member {member.MemberId}, character not found...");
                    DatabaseAccessor.Instance.Delete(member);
                }
            }

            foreach (var guildRecord in DatabaseAccessor.Instance.Select<GuildRecord>(GuildRelator.GetGuilds))
                if (_guildMembers.Values.Where(x => x.GuildId == guildRecord.Id).ToArray() is { Length: > 0 } guildMembers)
                    _guilds.TryAdd(guildRecord.Id, new(guildRecord, guildMembers));
                else
                {
                    Logger.Instance.LogInfo($"Deletion of guild {guildRecord.Id}, no members founded...");
                    DatabaseAccessor.Instance.Delete(guildRecord);
                }

            if (_guilds.Count is not 0)
            {
                var maxId = _guilds.Keys.Max();
                _idProvider.SetHighestId(maxId);

                for (var i = 1; i < maxId; i++)
                    if (!_guilds.ContainsKey(i))
                        _idProvider.Free(i);
            }
        }

        public static (byte, sbyte, short)? GetBoostInformations(GuildBoostType boostType) =>
            _guildBoostInfos.ContainsKey(boostType) ? _guildBoostInfos[boostType] : default;

        public bool IsEmblemCorrect(GuildEmblem emblem) =>
            _availablesShapes.Contains(emblem.symbolShape) && _availablesBackgrounds.Contains(emblem.backgroundShape) &&
            ColorExtensions.TryParse(emblem.symbolColor, out _) && ColorExtensions.TryParse(emblem.backgroundColor, out _) &&
            _guilds.Values.FirstOrDefault(x => x.Symbol == emblem.symbolShape && x.SymbolColor == emblem.symbolColor &&
                x.Background == emblem.backgroundShape && x.BackgroundColor == emblem.backgroundColor) is null;

        public Guild? GetGuildById(int guildId) =>
            _guilds.ContainsKey(guildId) ? _guilds[guildId] : default;

        public Guild? GetGuild(Predicate<Guild> p) =>
            _guilds.Values.FirstOrDefault(x => p(x));

        public GuildMember? GetGuildMemberById(int id) =>
            _guildMembers.ContainsKey(id) ? _guildMembers[id] : default;

        public Guild? CreateGuild(string name, GuildEmblem emblem)
        {
            var guild = new Guild(new GuildRecord()
            {
                Id = _idProvider.Generate(),
                Name = name,
                Symbol = emblem.symbolShape,
                SymbolColor = emblem.symbolColor,
                Background = emblem.backgroundShape,
                BackgroundColor = emblem.backgroundColor,
                MaxTaxCollectors = BaseMaxTaxCollector,
                TaxCollectorPods = BaseTaxCollectorPods,
                TaxCollectorProspecting = BaseTaxCollectorProspecting,
                SpellsLevelsCSV = string.Join(';', _taxCollectorsSpells.Select(x => $"{x},0"))
            }, Enumerable.Empty<GuildMember>());

            return _guilds.TryAdd(guild.Id, guild) ? guild : default;
        }

        public void DeleteGuild(Guild guild)
        {
            _guildsToDelete.Enqueue(guild);
            _guilds.Remove(guild.Id, out _);
        }

        public GuildMember? CreateGuildMember(Character character, Guild guild)
        {
            var member = new GuildMember(new()
            {
                GuildId = guild.Id,
                MemberId = character.Id,
                MemberRank = GuildRank.Essai
            });

            return _guildMembers.TryAdd(member.MemberId, member) ? member : default;
        }

        public void DeleteGuildMember(GuildMember member)
        {
            _guildMembersToDelete.Enqueue(member);
            _guildMembers.TryRemove(member.MemberId, out _);
        }

        public void Save()
        {
            foreach (var guild in _guilds.Values)
                guild.Save();

            while (_guildsToDelete.TryDequeue(out var oldGuild))
                oldGuild.Delete();

            foreach (var member in _guildMembers.Values)
                member.Save();

            while (_guildMembersToDelete.TryDequeue(out var oldMember))
                oldMember.Delete();

            foreach (var memberInfos in _guildMembers.Values)
                if (!_guilds.ContainsKey(memberInfos.GuildId))
                    memberInfos.Delete();
        }
    }
}
