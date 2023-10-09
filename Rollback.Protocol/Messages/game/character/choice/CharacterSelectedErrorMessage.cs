using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharacterSelectedErrorMessage : Message
	{
        public const int Id = 5836;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterSelectedErrorMessage()
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
