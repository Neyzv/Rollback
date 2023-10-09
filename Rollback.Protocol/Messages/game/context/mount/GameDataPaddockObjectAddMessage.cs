using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameDataPaddockObjectAddMessage : Message
	{
        public PaddockItem paddockItemDescription;

        public const int Id = 5990;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameDataPaddockObjectAddMessage()
        {
        }
        public GameDataPaddockObjectAddMessage(PaddockItem paddockItemDescription)
        {
            this.paddockItemDescription = paddockItemDescription;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            paddockItemDescription.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            paddockItemDescription = new PaddockItem();
            paddockItemDescription.Deserialize(reader);
		}
	}
}
