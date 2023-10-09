using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatSmileyMessage : Message
	{
        public int entityId;
        public sbyte smileyId;

        public const int Id = 801;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatSmileyMessage()
        {
        }
        public ChatSmileyMessage(int entityId, sbyte smileyId)
        {
            this.entityId = entityId;
            this.smileyId = smileyId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteSByte(smileyId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            entityId = reader.ReadInt();
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
		}
	}
}
