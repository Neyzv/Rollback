using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record AdditionalTaxCollectorInformations
    {
        public string CollectorCallerName;
        public int date;
        public const short Id = 165;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public AdditionalTaxCollectorInformations()
        {
        }
        public AdditionalTaxCollectorInformations(string CollectorCallerName, int date)
        {
            this.CollectorCallerName = CollectorCallerName;
            this.date = date;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(CollectorCallerName);
            writer.WriteInt(date);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            CollectorCallerName = reader.ReadString();
            date = reader.ReadInt();
            if (date < 0)
                throw new Exception("Forbidden value on date = " + date + ", it doesn't respect the following condition : date < 0");
        }
    }
}

