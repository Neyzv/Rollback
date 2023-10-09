using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PrismFightAttackerAddMessage : Message
	{
        public double fightId;
        public CharacterMinimalPlusLookAndGradeInformations[] charactersDescription;

        public const int Id = 5893;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismFightAttackerAddMessage()
        {
        }
        public PrismFightAttackerAddMessage(double fightId, CharacterMinimalPlusLookAndGradeInformations[] charactersDescription)
        {
            this.fightId = fightId;
            this.charactersDescription = charactersDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(fightId);
            writer.WriteUShort((ushort)charactersDescription.Length);
            foreach (var entry in charactersDescription)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadDouble();
            var limit = reader.ReadUShort();
            charactersDescription = new CharacterMinimalPlusLookAndGradeInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 charactersDescription[i] = new CharacterMinimalPlusLookAndGradeInformations();
                 charactersDescription[i].Deserialize(reader);
            }
		}
	}
}
