using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record QuestObjectiveValidatedMessage : Message
	{
        public ushort questId;
        public ushort objectiveId;

        public const int Id = 6098;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public QuestObjectiveValidatedMessage()
        {
        }
        public QuestObjectiveValidatedMessage(ushort questId, ushort objectiveId)
        {
            this.questId = questId;
            this.objectiveId = objectiveId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(questId);
            writer.WriteUShort(objectiveId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            questId = reader.ReadUShort();
            if (questId < 0 || questId > 65535)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
            objectiveId = reader.ReadUShort();
            if (objectiveId < 0 || objectiveId > 65535)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0 || objectiveId > 65535");
		}
	}
}
