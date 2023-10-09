using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ObjectItemToSellInBid : ObjectItemToSell
    {
        public short unsoldDelay;
        public new const short Id = 164;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectItemToSellInBid()
        {
        }
        public ObjectItemToSellInBid(short objectGID, ObjectEffect[] effects, int objectUID, int quantity, int objectPrice, short unsoldDelay) : base(objectGID, effects, objectUID, quantity, objectPrice)
        {
            this.unsoldDelay = unsoldDelay;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(unsoldDelay);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            unsoldDelay = reader.ReadShort();
            if (unsoldDelay < 0)
                throw new Exception("Forbidden value on unsoldDelay = " + unsoldDelay + ", it doesn't respect the following condition : unsoldDelay < 0");
        }
    }
}