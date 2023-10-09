using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextQuitMessage : Message
	{
        public const int Id = 255;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextQuitMessage()
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
