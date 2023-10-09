using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record ObjectEffectString : ObjectEffect
    {
        public string value;
        public new const short Id = 74;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectEffectString()
        {
        }
        public ObjectEffectString(short actionId, string value) : base(actionId)
        {
            this.value = value;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(value);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadString();
        }
    }
}

