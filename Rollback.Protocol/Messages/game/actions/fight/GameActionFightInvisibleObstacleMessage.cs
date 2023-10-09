using Rollback.Common.IO.Binary;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightInvisibleObstacleMessage : AbstractGameActionMessage
    {
        public int sourceSpellId;

        public new const int Id = 5820;
        public override uint MessageId
        {
            get { return Id; }
        }

        public GameActionFightInvisibleObstacleMessage()
        {
        }
        public GameActionFightInvisibleObstacleMessage(short actionId, int sourceId, int sourceSpellId) : base(actionId, sourceId)
        {
            this.sourceSpellId = sourceSpellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(sourceSpellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            sourceSpellId = reader.ReadInt();
            if (sourceSpellId < 0)
                throw new Exception("Forbidden value on sourceSpellId = " + sourceSpellId + ", it doesn't respect the following condition : sourceSpellId < 0");
        }
    }
}
