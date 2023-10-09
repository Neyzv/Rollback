using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ObjectEffect
    {
        public short actionId;
        public const short Id = 76;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public ObjectEffect()
        {
        }
        public ObjectEffect(short actionId)
        {
            this.actionId = actionId;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(actionId);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
        }
    }
}
