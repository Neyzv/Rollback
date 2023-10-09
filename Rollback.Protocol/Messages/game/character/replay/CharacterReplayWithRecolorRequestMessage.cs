using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterReplayWithRecolorRequestMessage : CharacterReplayRequestMessage
	{
        public int[] indexedColor;

        public new const int Id = 6111;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterReplayWithRecolorRequestMessage()
        {
        }
        public CharacterReplayWithRecolorRequestMessage(int characterId, int[] indexedColor) : base(characterId)
        {
            this.indexedColor = indexedColor;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)indexedColor.Length);
            foreach (var entry in indexedColor)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            indexedColor = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 indexedColor[i] = reader.ReadInt();
            }
		}
	}
}
