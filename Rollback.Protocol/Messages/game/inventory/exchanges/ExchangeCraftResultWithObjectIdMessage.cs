using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
	{
        public int objectGenericId;

        public new const int Id = 6000;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeCraftResultWithObjectIdMessage()
        {
        }
        public ExchangeCraftResultWithObjectIdMessage(sbyte craftResult, int objectGenericId) : base(craftResult)
        {
            this.objectGenericId = objectGenericId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectGenericId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            objectGenericId = reader.ReadInt();
            if (objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
		}
	}
}
