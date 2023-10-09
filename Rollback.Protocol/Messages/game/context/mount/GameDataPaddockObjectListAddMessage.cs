using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameDataPaddockObjectListAddMessage : Message
    {
        public PaddockItem[] paddockItemDescription;

        public const int Id = 5992;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameDataPaddockObjectListAddMessage()
        {
        }
        public GameDataPaddockObjectListAddMessage(PaddockItem[] paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)paddockItemDescription.Length);
            foreach (var entry in paddockItemDescription)
            {
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            paddockItemDescription = new PaddockItem[limit];
            for (int i = 0; i < limit; i++)
            {
                paddockItemDescription[i] = new PaddockItem();
                paddockItemDescription[i].Deserialize(reader);
            }
        }
    }
}
