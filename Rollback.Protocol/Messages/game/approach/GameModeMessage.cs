using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameModeMessage : Message
	{
        public sbyte mode;

        public const int Id = 6003;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameModeMessage()
        {
        }
        public GameModeMessage(sbyte mode)
        {
            this.mode = mode;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(mode);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mode = reader.ReadSByte();
            if (mode < 0)
                throw new Exception("Forbidden value on mode = " + mode + ", it doesn't respect the following condition : mode < 0");
		}
	}
}
