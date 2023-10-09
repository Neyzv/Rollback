using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record NpcDialogQuestionMessage : Message
	{
        public short messageId;
        public string[] dialogParams;
        public short[] visibleReplies;

        public const int Id = 5617;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NpcDialogQuestionMessage()
        {
        }
        public NpcDialogQuestionMessage(short messageId, string[] dialogParams, short[] visibleReplies)
        {
            this.messageId = messageId;
            this.dialogParams = dialogParams;
            this.visibleReplies = visibleReplies;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(messageId);
            writer.WriteUShort((ushort)dialogParams.Length);
            foreach (var entry in dialogParams)
            {
                 writer.WriteString(entry);
            }
            writer.WriteUShort((ushort)visibleReplies.Length);
            foreach (var entry in visibleReplies)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            messageId = reader.ReadShort();
            if (messageId < 0)
                throw new Exception("Forbidden value on messageId = " + messageId + ", it doesn't respect the following condition : messageId < 0");
            var limit = reader.ReadUShort();
            dialogParams = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 dialogParams[i] = reader.ReadString();
            }
            limit = reader.ReadUShort();
            visibleReplies = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 visibleReplies[i] = reader.ReadShort();
            }
		}
	}
}
