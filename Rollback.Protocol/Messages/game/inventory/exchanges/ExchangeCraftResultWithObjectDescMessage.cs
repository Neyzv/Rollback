using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record ExchangeCraftResultWithObjectDescMessage : ExchangeCraftResultMessage
	{
        public ObjectItemMinimalInformation objectInfo;

        public new const int Id = 5999;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ExchangeCraftResultWithObjectDescMessage()
        {
        }
        public ExchangeCraftResultWithObjectDescMessage(sbyte craftResult, ObjectItemMinimalInformation objectInfo) : base(craftResult)
        {
            this.objectInfo = objectInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            objectInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            objectInfo = new ObjectItemMinimalInformation();
            objectInfo.Deserialize(reader);
		}
	}
}
