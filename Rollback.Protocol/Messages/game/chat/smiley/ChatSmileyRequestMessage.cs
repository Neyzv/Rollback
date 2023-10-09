using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ChatSmileyRequestMessage : Message
	{
        public sbyte smileyId;

        public const int Id = 800;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ChatSmileyRequestMessage()
        {
        }
        public ChatSmileyRequestMessage(sbyte smileyId)
        {
            this.smileyId = smileyId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(smileyId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
		}
	}
}
