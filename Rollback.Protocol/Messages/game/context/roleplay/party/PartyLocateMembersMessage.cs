using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record PartyLocateMembersMessage : Message
	{
        public int[] geopositions;

        public const int Id = 5595;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyLocateMembersMessage()
        {
        }
        public PartyLocateMembersMessage(int[] geopositions)
        {
            this.geopositions = geopositions;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)geopositions.Length);
            foreach (var entry in geopositions)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            geopositions = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 geopositions[i] = reader.ReadInt();
            }
		}
	}
}
