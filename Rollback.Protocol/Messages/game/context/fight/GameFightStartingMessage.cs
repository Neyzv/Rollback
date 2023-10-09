using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameFightStartingMessage : Message
	{
        public sbyte fightType;

        public const int Id = 700;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightStartingMessage()
        {
        }
        public GameFightStartingMessage(sbyte fightType)
        {
            this.fightType = fightType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(fightType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
		}
	}
}
