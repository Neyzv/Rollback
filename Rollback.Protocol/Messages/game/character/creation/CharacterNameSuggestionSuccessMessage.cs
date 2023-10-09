using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterNameSuggestionSuccessMessage : Message
	{
        public string suggestion;

        public const int Id = 5544;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterNameSuggestionSuccessMessage()
        {
        }
        public CharacterNameSuggestionSuccessMessage(string suggestion)
        {
            this.suggestion = suggestion;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(suggestion);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            suggestion = reader.ReadString();
		}
	}
}
