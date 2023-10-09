using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record EmotePlayMessage : EmotePlayAbstractMessage
	{
        public int actorId;

        public new const int Id = 5683;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmotePlayMessage()
        {
        }
        public EmotePlayMessage(sbyte emoteId, byte duration, int actorId) : base(emoteId, duration)
        {
            this.actorId = actorId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(actorId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            actorId = reader.ReadInt();
		}
	}
}
