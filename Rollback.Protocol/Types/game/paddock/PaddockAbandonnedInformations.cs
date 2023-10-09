using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record PaddockAbandonnedInformations : PaddockBuyableInformations
    {
        public int guildId;
        public new const short Id = 133;
        public override short TypeId
        {
            get { return Id; }
        }
        public PaddockAbandonnedInformations()
        {
        }
        public PaddockAbandonnedInformations(short maxOutdoorMount, short maxItems, int price, int guildId) : base(maxOutdoorMount, maxItems, price)
        {
            this.guildId = guildId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(guildId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            guildId = reader.ReadInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
        }
    }
}