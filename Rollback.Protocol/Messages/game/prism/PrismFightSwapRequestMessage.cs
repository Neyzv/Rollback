using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PrismFightSwapRequestMessage : Message
	{
        public int targetId;

        public const int Id = 5901;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismFightSwapRequestMessage()
        {
        }
        public PrismFightSwapRequestMessage(int targetId)
        {
            this.targetId = targetId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(targetId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
		}
	}
}
