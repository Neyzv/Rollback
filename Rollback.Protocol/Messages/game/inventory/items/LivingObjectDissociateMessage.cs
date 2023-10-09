using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LivingObjectDissociateMessage : Message
	{
        public int livingUID;
        public byte livingPosition;

        public const int Id = 5723;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LivingObjectDissociateMessage()
        {
        }
        public LivingObjectDissociateMessage(int livingUID, byte livingPosition)
        {
            this.livingUID = livingUID;
            this.livingPosition = livingPosition;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(livingUID);
            writer.WriteByte(livingPosition);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            livingUID = reader.ReadInt();
            if (livingUID < 0)
                throw new Exception("Forbidden value on livingUID = " + livingUID + ", it doesn't respect the following condition : livingUID < 0");
            livingPosition = reader.ReadByte();
            if (livingPosition < 0 || livingPosition > 255)
                throw new Exception("Forbidden value on livingPosition = " + livingPosition + ", it doesn't respect the following condition : livingPosition < 0 || livingPosition > 255");
		}
	}
}
