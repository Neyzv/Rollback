using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record SpellMovementMessage : Message
	{
        public short spellId;
        public byte position;

        public const int Id = 5746;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellMovementMessage()
        {
        }
        public SpellMovementMessage(short spellId, byte position)
        {
            this.spellId = spellId;
            this.position = position;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(spellId);
            writer.WriteByte(position);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            position = reader.ReadByte();
            if (position < 63 || position > 255)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 63 || position > 255");
		}
	}
}
