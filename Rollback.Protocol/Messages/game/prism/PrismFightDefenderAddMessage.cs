using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PrismFightDefenderAddMessage : Message
	{
        public double fightId;
        public CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
        public bool inMain;

        public const int Id = 5895;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismFightDefenderAddMessage()
        {
        }
        public PrismFightDefenderAddMessage(double fightId, CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations, bool inMain)
        {
            this.fightId = fightId;
            this.fighterMovementInformations = fighterMovementInformations;
            this.inMain = inMain;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteDouble(fightId);
            fighterMovementInformations.Serialize(writer);
            writer.WriteBoolean(inMain);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadDouble();
            fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
            fighterMovementInformations.Deserialize(reader);
            inMain = reader.ReadBoolean();
		}
	}
}
