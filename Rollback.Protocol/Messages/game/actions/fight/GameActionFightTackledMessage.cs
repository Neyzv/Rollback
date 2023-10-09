using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameActionFightTackledMessage : AbstractGameActionMessage
	{

        public new const int Id = 1004;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightTackledMessage()
        {
        }
        public GameActionFightTackledMessage(short actionId, int sourceId) : base(actionId, sourceId)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
		}
	}
}
