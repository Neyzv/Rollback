using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MountEmoteIconUsedOkMessage : Message
	{
        public int mountId;
        public sbyte reactionType;

        public const int Id = 5978;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountEmoteIconUsedOkMessage()
        {
        }
        public MountEmoteIconUsedOkMessage(int mountId, sbyte reactionType)
        {
            this.mountId = mountId;
            this.reactionType = reactionType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(mountId);
            writer.WriteSByte(reactionType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mountId = reader.ReadInt();
            reactionType = reader.ReadSByte();
            if (reactionType < 0)
                throw new Exception("Forbidden value on reactionType = " + reactionType + ", it doesn't respect the following condition : reactionType < 0");
		}
	}
}
