using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record NpcDialogReplyMessage : Message
	{
        public short replyId;

        public const int Id = 5616;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NpcDialogReplyMessage()
        {
        }
        public NpcDialogReplyMessage(short replyId)
        {
            this.replyId = replyId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(replyId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            replyId = reader.ReadShort();
            if (replyId < 0)
                throw new Exception("Forbidden value on replyId = " + replyId + ", it doesn't respect the following condition : replyId < 0");
		}
	}
}
