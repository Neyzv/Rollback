using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameEntitiesDispositionMessage : Message
	{
        public IdentifiedEntityDispositionInformations[] dispositions;

        public const int Id = 5696;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameEntitiesDispositionMessage()
        {
        }
        public GameEntitiesDispositionMessage(IdentifiedEntityDispositionInformations[] dispositions)
        {
            this.dispositions = dispositions;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)dispositions.Length);
            foreach (var entry in dispositions)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            dispositions = new IdentifiedEntityDispositionInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 dispositions[i] = new IdentifiedEntityDispositionInformations();
                 dispositions[i].Deserialize(reader);
            }
		}
	}
}
