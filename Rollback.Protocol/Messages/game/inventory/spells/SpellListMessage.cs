using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record SpellListMessage : Message
	{
        public bool spellPrevisualization;
        public SpellItem[] spells;

        public const int Id = 1200;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellListMessage()
        {
        }
        public SpellListMessage(bool spellPrevisualization, SpellItem[] spells)
        {
            this.spellPrevisualization = spellPrevisualization;
            this.spells = spells;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(spellPrevisualization);
            writer.WriteUShort((ushort)spells.Length);
            foreach (var entry in spells)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellPrevisualization = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            spells = new SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 spells[i] = new SpellItem();
                 spells[i].Deserialize(reader);
            }
		}
	}
}
