using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LockableStateUpdateHouseDoorMessage : LockableStateUpdateAbstractMessage
	{
        public int houseId;

        public new const int Id = 5668;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableStateUpdateHouseDoorMessage()
        {
        }
        public LockableStateUpdateHouseDoorMessage(bool locked, int houseId) : base(locked)
        {
            this.houseId = houseId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(houseId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            houseId = reader.ReadInt();
		}
	}
}
