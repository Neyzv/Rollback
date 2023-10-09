using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PrismFightStateUpdateMessage : Message
	{
        public sbyte state;

        public const int Id = 6040;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismFightStateUpdateMessage()
        {
        }
        public PrismFightStateUpdateMessage(sbyte state)
        {
            this.state = state;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(state);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
		}
	}
}
