using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record TaxCollectorFightersInformation
    {
        public int collectorId;
        public CharacterMinimalPlusLookInformations[] allyCharactersInformations;
        public CharacterMinimalPlusLookInformations[] enemyCharactersInformations;
        public const short Id = 169;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public TaxCollectorFightersInformation()
        {
        }
        public TaxCollectorFightersInformation(int collectorId, CharacterMinimalPlusLookInformations[] allyCharactersInformations, CharacterMinimalPlusLookInformations[] enemyCharactersInformations)
        {
            this.collectorId = collectorId;
            this.allyCharactersInformations = allyCharactersInformations;
            this.enemyCharactersInformations = enemyCharactersInformations;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(collectorId);
            writer.WriteUShort((ushort)allyCharactersInformations.Length);
            foreach (var entry in allyCharactersInformations)
            {
                entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)enemyCharactersInformations.Length);
            foreach (var entry in enemyCharactersInformations)
            {
                entry.Serialize(writer);
            }
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            collectorId = reader.ReadInt();
            var limit = reader.ReadUShort();
            allyCharactersInformations = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                allyCharactersInformations[i] = new CharacterMinimalPlusLookInformations();
                allyCharactersInformations[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            enemyCharactersInformations = new CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                enemyCharactersInformations[i] = new CharacterMinimalPlusLookInformations();
                enemyCharactersInformations[i].Deserialize(reader);
            }
        }
    }
}
