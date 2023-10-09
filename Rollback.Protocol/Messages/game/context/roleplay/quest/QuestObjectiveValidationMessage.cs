using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record QuestObjectiveValidationMessage : Message
	{
        public short questId;
        public short objectiveId;

        public const int Id = 6085;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestObjectiveValidationMessage()
        {
        }
        public QuestObjectiveValidationMessage(short questId, short objectiveId)
        {
            this.questId = questId;
            this.objectiveId = objectiveId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(questId);
            writer.WriteShort(objectiveId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            questId = reader.ReadShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
            objectiveId = reader.ReadShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
		}
	}
}
