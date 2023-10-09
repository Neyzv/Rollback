using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record AlignmentSubAreasListMessage : Message
	{
        public short[] angelsSubAreas;
        public short[] evilsSubAreas;

        public const int Id = 6059;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AlignmentSubAreasListMessage()
        {
        }
        public AlignmentSubAreasListMessage(short[] angelsSubAreas, short[] evilsSubAreas)
        {
            this.angelsSubAreas = angelsSubAreas;
            this.evilsSubAreas = evilsSubAreas;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)angelsSubAreas.Length);
            foreach (var entry in angelsSubAreas)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)evilsSubAreas.Length);
            foreach (var entry in evilsSubAreas)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            angelsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 angelsSubAreas[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            evilsSubAreas = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 evilsSubAreas[i] = reader.ReadShort();
            }
		}
	}
}
