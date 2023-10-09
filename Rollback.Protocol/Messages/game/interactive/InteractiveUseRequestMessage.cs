using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record InteractiveUseRequestMessage : Message
	{
        public int elemId;
        public short skillId;

        public const int Id = 5001;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public InteractiveUseRequestMessage()
        {
        }
        public InteractiveUseRequestMessage(int elemId, short skillId)
        {
            this.elemId = elemId;
            this.skillId = skillId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(elemId);
            writer.WriteShort(skillId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            elemId = reader.ReadInt();
            if (elemId < 0)
                throw new Exception("Forbidden value on elemId = " + elemId + ", it doesn't respect the following condition : elemId < 0");
            skillId = reader.ReadShort();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
		}
	}
}
