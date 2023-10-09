using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PauseDialogMessage : Message
	{
        public const int Id = 6012;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PauseDialogMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
