using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameRolePlayFreeSoulRequestMessage : Message
	{
        public const int Id = 745;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayFreeSoulRequestMessage()
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
