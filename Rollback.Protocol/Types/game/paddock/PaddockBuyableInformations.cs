using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record PaddockBuyableInformations : PaddockInformations
    {
        public int price;
        public new const short Id = 130;
        public override short TypeId
        {
            get { return Id; }
        }
        public PaddockBuyableInformations()
        {
        }
        public PaddockBuyableInformations(short maxOutdoorMount, short maxItems, int price) : base(maxOutdoorMount, maxItems)
        {
            this.price = price;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(price);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadInt();
            if (price < 0)
                throw new Exception("Forbidden value on price = " + price + ", it doesn't respect the following condition : price < 0");
        }
    }
}