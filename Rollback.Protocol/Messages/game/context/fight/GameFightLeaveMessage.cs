using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameFightLeaveMessage : Message
	{
        public int charId;

        public const int Id = 721;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightLeaveMessage()
        {
        }
        public GameFightLeaveMessage(int charId)
        {
            this.charId = charId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(charId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            charId = reader.ReadInt();
		}
	}
}
