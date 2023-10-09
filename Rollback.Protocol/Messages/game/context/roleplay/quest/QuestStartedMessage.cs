using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record QuestStartedMessage : Message
	{
        public ushort questId;

        public const int Id = 6091;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestStartedMessage()
        {
        }
        public QuestStartedMessage(ushort questId)
        {
            this.questId = questId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(questId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            questId = reader.ReadUShort();
            if (questId < 0 || questId > 65535)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
		}
	}
}
