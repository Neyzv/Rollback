using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ValidateSpellForgetMessage : Message
	{
        public short spellId;

        public const int Id = 1700;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ValidateSpellForgetMessage()
        {
        }
        public ValidateSpellForgetMessage(short spellId)
        {
            this.spellId = spellId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(spellId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            spellId = reader.ReadShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
		}
	}
}
