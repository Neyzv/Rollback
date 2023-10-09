using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record TaxCollectorDialogQuestionBasicMessage : Message
	{
        public string guildName;

        public const int Id = 5619;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TaxCollectorDialogQuestionBasicMessage()
        {
        }
        public TaxCollectorDialogQuestionBasicMessage(string guildName)
        {
            this.guildName = guildName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(guildName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            guildName = reader.ReadString();
		}
	}
}
