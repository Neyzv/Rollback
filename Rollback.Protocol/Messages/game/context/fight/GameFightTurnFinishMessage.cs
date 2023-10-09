using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameFightTurnFinishMessage : Message
	{
        public const int Id = 718;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightTurnFinishMessage()
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
