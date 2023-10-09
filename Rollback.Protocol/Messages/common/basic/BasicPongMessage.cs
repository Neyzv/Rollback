using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicPongMessage : Message
	{
        public bool quiet;

        public const int Id = 183;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicPongMessage()
        {
        }
        public BasicPongMessage(bool quiet)
        {
            this.quiet = quiet;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(quiet);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            quiet = reader.ReadBoolean();
		}
	}
}
