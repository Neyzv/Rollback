using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayGameOverMessage : Message
	{
        public const int Id = 746;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayGameOverMessage()
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
