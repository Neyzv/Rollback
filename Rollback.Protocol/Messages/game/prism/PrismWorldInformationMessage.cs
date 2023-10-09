using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record PrismWorldInformationMessage : Message
	{
        public int nbSubOwned;
        public int subTotal;
        public int maxSub;
        public PrismSubAreaInformation[] subAreasInformation;
        public int nbConqsOwned;
        public int conqsTotal;
        public PrismConquestInformation[] conquetesInformation;

        public const int Id = 5854;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismWorldInformationMessage()
        {
        }
        public PrismWorldInformationMessage(int nbSubOwned, int subTotal, int maxSub, PrismSubAreaInformation[] subAreasInformation, int nbConqsOwned, int conqsTotal, PrismConquestInformation[] conquetesInformation)
        {
            this.nbSubOwned = nbSubOwned;
            this.subTotal = subTotal;
            this.maxSub = maxSub;
            this.subAreasInformation = subAreasInformation;
            this.nbConqsOwned = nbConqsOwned;
            this.conqsTotal = conqsTotal;
            this.conquetesInformation = conquetesInformation;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(nbSubOwned);
            writer.WriteInt(subTotal);
            writer.WriteInt(maxSub);
            writer.WriteUShort((ushort)subAreasInformation.Length);
            foreach (var entry in subAreasInformation)
            {
                 entry.Serialize(writer);
            }
            writer.WriteInt(nbConqsOwned);
            writer.WriteInt(conqsTotal);
            writer.WriteUShort((ushort)conquetesInformation.Length);
            foreach (var entry in conquetesInformation)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            nbSubOwned = reader.ReadInt();
            if (nbSubOwned < 0)
                throw new Exception("Forbidden value on nbSubOwned = " + nbSubOwned + ", it doesn't respect the following condition : nbSubOwned < 0");
            subTotal = reader.ReadInt();
            if (subTotal < 0)
                throw new Exception("Forbidden value on subTotal = " + subTotal + ", it doesn't respect the following condition : subTotal < 0");
            maxSub = reader.ReadInt();
            if (maxSub < 0)
                throw new Exception("Forbidden value on maxSub = " + maxSub + ", it doesn't respect the following condition : maxSub < 0");
            var limit = reader.ReadUShort();
            subAreasInformation = new PrismSubAreaInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 subAreasInformation[i] = new PrismSubAreaInformation();
                 subAreasInformation[i].Deserialize(reader);
            }
            nbConqsOwned = reader.ReadInt();
            if (nbConqsOwned < 0)
                throw new Exception("Forbidden value on nbConqsOwned = " + nbConqsOwned + ", it doesn't respect the following condition : nbConqsOwned < 0");
            conqsTotal = reader.ReadInt();
            if (conqsTotal < 0)
                throw new Exception("Forbidden value on conqsTotal = " + conqsTotal + ", it doesn't respect the following condition : conqsTotal < 0");
            limit = reader.ReadUShort();
            conquetesInformation = new PrismConquestInformation[limit];
            for (int i = 0; i < limit; i++)
            {
                 conquetesInformation[i] = new PrismConquestInformation();
                 conquetesInformation[i].Deserialize(reader);
            }
		}
	}
}
