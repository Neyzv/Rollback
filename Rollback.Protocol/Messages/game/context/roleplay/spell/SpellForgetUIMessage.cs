using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record SpellForgetUIMessage : Message
	{
        public bool open;

        public const int Id = 5565;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SpellForgetUIMessage()
        {
        }
        public SpellForgetUIMessage(bool open)
        {
            this.open = open;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(open);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            open = reader.ReadBoolean();
		}
	}
}
