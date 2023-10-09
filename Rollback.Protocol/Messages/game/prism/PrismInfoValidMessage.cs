using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PrismInfoValidMessage : Message
	{
        public ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;

        public const int Id = 5858;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismInfoValidMessage()
        {
        }
        public PrismInfoValidMessage(ProtectedEntityWaitingForHelpInfo waitingForHelpInfo)
        {
            this.waitingForHelpInfo = waitingForHelpInfo;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            waitingForHelpInfo.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
		}
	}
}
