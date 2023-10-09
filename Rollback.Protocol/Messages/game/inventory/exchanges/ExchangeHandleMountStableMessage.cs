using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeHandleMountStableMessage : Message
	{
        public sbyte actionType;
        public int rideId;

        public const int Id = 5965;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeHandleMountStableMessage()
        {
        }
        public ExchangeHandleMountStableMessage(sbyte actionType, int rideId)
        {
            this.actionType = actionType;
            this.rideId = rideId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(actionType);
            writer.WriteInt(rideId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            actionType = reader.ReadSByte();
            rideId = reader.ReadInt();
            if (rideId < 0)
                throw new Exception("Forbidden value on rideId = " + rideId + ", it doesn't respect the following condition : rideId < 0");
		}
	}
}
