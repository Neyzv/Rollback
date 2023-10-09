using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameActionFightCastRequestMessage : Message
	{
        public short spellId;
        public short cellId;

        public const int Id = 1005;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameActionFightCastRequestMessage()
        {
        }
        public GameActionFightCastRequestMessage(short spellId, short cellId)
        {
            this.spellId = spellId;
            this.cellId = cellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(spellId);
            writer.WriteShort(cellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            cellId = reader.ReadShort();
            if (cellId < -1 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < -1 || cellId > 559");
		}
	}
}
