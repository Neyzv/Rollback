using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameDataPlayFarmObjectAnimationMessage : Message
	{
        public short[] cellId;

        public const int Id = 6026;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameDataPlayFarmObjectAnimationMessage()
        {
        }
        public GameDataPlayFarmObjectAnimationMessage(short[] cellId)
        {
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)cellId.Length);
            foreach (var entry in cellId)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            cellId = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 cellId[i] = reader.ReadShort();
            }
		}
	}
}
