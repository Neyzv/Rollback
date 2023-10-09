using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
    public record TaxCollectorListMessage : Message
    {
        public sbyte nbcollectorMax;
        public short taxCollectorHireCost;
        public TaxCollectorInformations[] informations;
        public TaxCollectorFightersInformation[] fightersInformations;

        public const int Id = 5930;
        public override uint MessageId
        {
            get { return Id; }
        }
        public TaxCollectorListMessage()
        {
        }
        public TaxCollectorListMessage(sbyte nbcollectorMax, short taxCollectorHireCost, TaxCollectorInformations[] informations, TaxCollectorFightersInformation[] fightersInformations)
        {
            this.nbcollectorMax = nbcollectorMax;
            this.taxCollectorHireCost = taxCollectorHireCost;
            this.informations = informations;
            this.fightersInformations = fightersInformations;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(nbcollectorMax);
            writer.WriteShort(taxCollectorHireCost);
            writer.WriteUShort((ushort)informations.Length);
            foreach (var entry in informations)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fightersInformations.Length);
            foreach (var entry in fightersInformations)
            {
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            nbcollectorMax = reader.ReadSByte();
            if (nbcollectorMax < 0)
                throw new Exception("Forbidden value on nbcollectorMax = " + nbcollectorMax + ", it doesn't respect the following condition : nbcollectorMax < 0");
            taxCollectorHireCost = reader.ReadShort();
            if (taxCollectorHireCost < 0)
                throw new Exception("Forbidden value on taxCollectorHireCost = " + taxCollectorHireCost + ", it doesn't respect the following condition : taxCollectorHireCost < 0");
            var limit = reader.ReadUShort();
            informations = new TaxCollectorInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                informations[i] = (TaxCollectorInformations)ProtocolManager.Instance.GetType(reader.ReadUShort());
                informations[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fightersInformations = new TaxCollectorFightersInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                fightersInformations[i] = new TaxCollectorFightersInformation();
                fightersInformations[i].Deserialize(reader);
            }
        }
    }
}
