using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record SequenceStartMessage : Message
	{
        public int authorId;
        public sbyte sequenceType;

        public const int Id = 955;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SequenceStartMessage()
        {
        }
        public SequenceStartMessage(int authorId, sbyte sequenceType)
        {
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(authorId);
            writer.WriteSByte(sequenceType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            authorId = reader.ReadInt();
            sequenceType = reader.ReadSByte();
		}
	}
}
