using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameFightTurnReadyMessage : Message
	{
        public bool isReady;

        public const int Id = 716;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightTurnReadyMessage()
        {
        }
        public GameFightTurnReadyMessage(bool isReady)
        {
            this.isReady = isReady;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(isReady);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            isReady = reader.ReadBoolean();
		}
	}
}
