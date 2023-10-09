using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record QuestListRequestMessage : Message
	{
        public const int Id = 5623;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestListRequestMessage()
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
