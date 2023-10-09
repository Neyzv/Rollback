using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameEntityDispositionMessage : Message
	{
        public IdentifiedEntityDispositionInformations disposition;

        public const int Id = 5693;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameEntityDispositionMessage()
        {
        }
        public GameEntityDispositionMessage(IdentifiedEntityDispositionInformations disposition)
        {
            this.disposition = disposition;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            disposition.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            disposition = new IdentifiedEntityDispositionInformations();
            disposition.Deserialize(reader);
		}
	}
}
