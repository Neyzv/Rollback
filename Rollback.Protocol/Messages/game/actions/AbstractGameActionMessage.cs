using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AbstractGameActionMessage : Message
    {
        public short actionId;
        public int sourceId;

        public const int Id = 1000;
        public override uint MessageId
        {
            get { return Id; }
        }
        public AbstractGameActionMessage()
        {
        }
        public AbstractGameActionMessage(short actionId, int sourceId)
        {
            this.actionId = actionId;
            this.sourceId = sourceId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(actionId);
            writer.WriteInt(sourceId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            sourceId = reader.ReadInt();
        }
    }
}
