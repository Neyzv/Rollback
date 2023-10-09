using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectFoundWhileRecoltingMessage : Message
	{
        public int genericId;
        public int quantity;
        public int ressourceGenericId;

        public const int Id = 6017;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectFoundWhileRecoltingMessage()
        {
        }
        public ObjectFoundWhileRecoltingMessage(int genericId, int quantity, int ressourceGenericId)
        {
            this.genericId = genericId;
            this.quantity = quantity;
            this.ressourceGenericId = ressourceGenericId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(genericId);
            writer.WriteInt(quantity);
            writer.WriteInt(ressourceGenericId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            genericId = reader.ReadInt();
            if (genericId < 0)
                throw new Exception("Forbidden value on genericId = " + genericId + ", it doesn't respect the following condition : genericId < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
            ressourceGenericId = reader.ReadInt();
            if (ressourceGenericId < 0)
                throw new Exception("Forbidden value on ressourceGenericId = " + ressourceGenericId + ", it doesn't respect the following condition : ressourceGenericId < 0");
		}
	}
}
