using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record CharactersListRequestMessage : Message
	{

        public const int Id = 150;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharactersListRequestMessage()
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
