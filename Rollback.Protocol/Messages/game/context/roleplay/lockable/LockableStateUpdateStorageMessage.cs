using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LockableStateUpdateStorageMessage : LockableStateUpdateAbstractMessage
	{
        public int mapId;
        public int elementId;

        public new const int Id = 5669;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableStateUpdateStorageMessage()
        {
        }
        public LockableStateUpdateStorageMessage(bool locked, int mapId, int elementId) : base(locked)
        {
            this.mapId = mapId;
            this.elementId = elementId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mapId);
            writer.WriteInt(elementId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            mapId = reader.ReadInt();
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
		}
	}
}
