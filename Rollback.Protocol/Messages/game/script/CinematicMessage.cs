using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CinematicMessage : Message
	{
        public short cinematicId;

        public const int Id = 6053;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CinematicMessage()
        {
        }
        public CinematicMessage(short cinematicId)
        {
            this.cinematicId = cinematicId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(cinematicId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            cinematicId = reader.ReadShort();
            if (cinematicId < 0)
                throw new Exception("Forbidden value on cinematicId = " + cinematicId + ", it doesn't respect the following condition : cinematicId < 0");
		}
	}
}
