using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GuildHousesInformationMessage : Message
	{
        public HouseInformationsForGuild[] HousesInformations;

        public const int Id = 5919;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildHousesInformationMessage()
        {
        }
        public GuildHousesInformationMessage(HouseInformationsForGuild[] HousesInformations)
        {
            this.HousesInformations = HousesInformations;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)HousesInformations.Length);
            foreach (var entry in HousesInformations)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            HousesInformations = new HouseInformationsForGuild[limit];
            for (int i = 0; i < limit; i++)
            {
                 HousesInformations[i] = new HouseInformationsForGuild();
                 HousesInformations[i].Deserialize(reader);
            }
		}
	}
}
