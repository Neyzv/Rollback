using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterNameSuggestionRequestMessage : Message
	{
        public const int Id = 162;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterNameSuggestionRequestMessage()
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
        }
        public override void Deserialize(BigEndianReader reader)
        {
		}
	}
}
