using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameMapChangeOrientationRequestMessage : Message
	{
        public sbyte direction;

        public const int Id = 945;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameMapChangeOrientationRequestMessage()
        {
        }
        public GameMapChangeOrientationRequestMessage(sbyte direction)
        {
            this.direction = direction;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(direction);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
		}
	}
}
