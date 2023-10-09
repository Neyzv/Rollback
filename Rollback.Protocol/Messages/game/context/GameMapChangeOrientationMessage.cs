using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameMapChangeOrientationMessage : Message
	{
        public int id;
        public sbyte direction;

        public const int Id = 946;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameMapChangeOrientationMessage()
        {
        }
        public GameMapChangeOrientationMessage(int id, sbyte direction)
        {
            this.id = id;
            this.direction = direction;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteSByte(direction);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
		}
	}
}
