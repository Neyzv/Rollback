using System.Drawing;
using System.Text.RegularExpressions;
using Rollback.Common.DesignPattern.Instance;
using Rollback.Common.Initialization;
using Rollback.Common.IO.Binary;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters.Breeds;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;
using Rollback.World.Network.IPC.Handlers.Characters;

namespace Rollback.World.Game.RolePlayActors.Characters
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        private static readonly Regex _nameRegex;

        private UniqueIdProvider _uniqueIdProvider;

        static CharacterManager() =>
            _nameRegex = new Regex("^(?=[A-Z][a-z]+(?:-[A-Za-z][a-z]+)?$)(?=.{3,31}$)", RegexOptions.Compiled);

        public CharacterManager() =>
            _uniqueIdProvider = new UniqueIdProvider();

        [Initializable(InitializationPriority.DatasManager, "Character id provider")]
        public void Initialize()
        {
            var ids = new HashSet<int>();
            foreach (var id in DatabaseAccessor.Instance.Select(CharacterRelator.GetCharactersIds))
                ids.Add((int)id.First().Value!);

            _uniqueIdProvider = new UniqueIdProvider(ids);
        }

        public static CharacterRecord? GetCharacterRecordById(int id) =>
            DatabaseAccessor.Instance.SelectSingle<CharacterRecord>(string.Format(CharacterRelator.GetCharacterById, id));

        public static CharacterRecord? GetCharacterRecordByName(string name) =>
            DatabaseAccessor.Instance.SelectSingle<CharacterRecord>(string.Format(CharacterRelator.GetCharacterByName, name));

        public static bool IsNameCorrect(string name) =>
            _nameRegex.IsMatch(name);

        public CharacterRecord? CreateCharacter(int accountId, string name, bool sex, int breedId, int[] colors)
        {
            CharacterRecord? record = default;
            var breed = BreedManager.Instance.GetBreedById(breedId);

            if (breed is not null)
            {
                var look = ActorLook.Parse(sex ? breed.FemaleLook : breed.MaleLook);
                for (byte i = 0; i < colors.Length; i++)
                    look.AddColor((byte)(i + 1), Color.FromArgb(colors[i]));

                var xp = ExperienceManager.Instance.GetCharacterExperienceForLevel(GeneralConfiguration.Instance.CharacterStartLevel);
                if (xp >= 0)
                {
                    record = new()
                    {
                        Id = _uniqueIdProvider.Generate(),
                        Name = name,
                        Breed = (BreedEnum)breed.Id,
                        Sex = sex,
                        BaseEntityLookString = look.ToString(),
                        Look = look,
                        LifeState = PlayerLifeStatusEnum.STATUS_ALIVE_AND_KICKING,
                        Experience = xp,
                        Kamas = GeneralConfiguration.Instance.CharacterStartKamas,
                        MapId = breed.StartMapId,
                        CellId = breed.StartCellId,
                        Direction = breed.StartDirection,
                        StatsPoints = (short)((GeneralConfiguration.Instance.CharacterStartLevel - 1) * 5),
                        SpellsPoints = (short)(GeneralConfiguration.Instance.CharacterStartLevel - 1),
                        Energy = 10000,
                        Health = 50 + GeneralConfiguration.Instance.CharacterStartLevel * 5,
                        AP = (byte)(GeneralConfiguration.Instance.CharacterStartLevel >= 100 ? 7 : 6),
                        MP = 3,
                        LastSelection = DateTime.Now,
                    };

                    DatabaseAccessor.Instance.Insert(record);

                    record = GetCharacterRecordByName(name);
                    if (record is not null)
                    {
                        byte pos = 64;
                        foreach (var spell in breed.Spells)
                        {
                            var spellTemp = SpellManager.Instance.GetSpellTemplateById(spell.SpellId);
                            if (spellTemp is not null && spellTemp.SpellLevels.Length > 0 && spellTemp.SpellLevels[0].MinPlayerLevel <= GeneralConfiguration.Instance.CharacterStartLevel)
                            {
                                DatabaseAccessor.Instance.Insert(new CharacterSpellRecord()
                                {
                                    OwnerId = record.Id,
                                    SpellId = spell.SpellId,
                                    SpellLevel = 1,
                                    Position = pos
                                });

                                pos++;
                            }
                        }

                        IPCCharacterHandler.SendCharacterAddedMessage(accountId, record.Id);
                    }
                }
            }

            return record;
        }

        public static bool DeleteCharacter(CharacterRecord characterRecord)
        {
            IPCCharacterHandler.SendCharacterDeletedMessage(characterRecord.Id);

            DatabaseAccessor.Instance.ExecuteNonQuery(string.Format(CharacterSpellRelator.DeleteByOwnerId, characterRecord.Id));
            DatabaseAccessor.Instance.ExecuteNonQuery(string.Format(CharacterItemRelator.DeleteByOwnerId, characterRecord.Id));

            if (GuildManager.Instance.GetGuildMemberById(characterRecord.Id) is { } guildMember)
            {
                foreach (var taxCollector in guildMember.Guild.GetTaxCollectors(x => x.Hirer.MemberId == characterRecord.Id))
                    taxCollector.Guild.RemoveTaxCollector(taxCollector.Id);

                guildMember.Guild.KickMember(guildMember.MemberId, guildMember.MemberId);
            }

            return DatabaseAccessor.Instance.Delete(characterRecord);
        }

        public static byte[] SerializeZaaps(Dictionary<int, Map> knownZaaps)
        {
            using var writer = new BigEndianWriter();
            foreach (var map in knownZaaps.Values)
                writer.WriteInt(map.Record.Id);

            return writer.Buffer;
        }

        public static Dictionary<int, Map> DeserializeZaaps(byte[] buffer)
        {
            var result = new Dictionary<int, Map>();
            using var reader = new BigEndianReader(buffer);
            while (reader.BytesAvailable >= sizeof(int))
            {
                var mapId = reader.ReadInt();
                var map = WorldManager.Instance.GetMapById(mapId)
                    ?? throw new Exception($"Invalid map id {mapId}...");

                result[map.Record.Id] = map;
            }

            return result;
        }
    }
}
