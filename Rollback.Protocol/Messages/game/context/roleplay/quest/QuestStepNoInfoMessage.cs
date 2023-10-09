using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record QuestStepNoInfoMessage : Message
	{
        public short questId;

        public const int Id = 5627;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestStepNoInfoMessage()
        {
        }
        public QuestStepNoInfoMessage(short questId)
        {
            this.questId = questId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(questId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            questId = reader.ReadShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
		}
	}
}
