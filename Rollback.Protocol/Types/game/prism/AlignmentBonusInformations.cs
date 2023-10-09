using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record AlignmentBonusInformations
    {
        public int pctbonus;
        public double grademult;
        public const short Id = 135;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public AlignmentBonusInformations()
        {
        }
        public AlignmentBonusInformations(int pctbonus, double grademult)
        {
            this.pctbonus = pctbonus;
            this.grademult = grademult;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(pctbonus);
            writer.WriteDouble(grademult);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            pctbonus = reader.ReadInt();
            if (pctbonus < 0)
                throw new Exception("Forbidden value on pctbonus = " + pctbonus + ", it doesn't respect the following condition : pctbonus < 0");
            grademult = reader.ReadDouble();
        }
    }
}
