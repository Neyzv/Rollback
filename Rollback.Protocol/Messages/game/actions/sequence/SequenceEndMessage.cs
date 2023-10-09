using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SequenceEndMessage : Message
	{
        public short actionId;
        public int authorId;
        public sbyte sequenceType;

        public const int Id = 956;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SequenceEndMessage()
        {
        }
        public SequenceEndMessage(short actionId, int authorId, sbyte sequenceType)
        {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(actionId);
            writer.WriteInt(authorId);
            writer.WriteSByte(sequenceType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            authorId = reader.ReadInt();
            sequenceType = reader.ReadSByte();
		}
	}
}
