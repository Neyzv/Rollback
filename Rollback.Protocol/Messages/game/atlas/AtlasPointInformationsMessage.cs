using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record AtlasPointInformationsMessage : Message
	{
        public AtlasPointsInformations type;

        public const int Id = 5956;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public AtlasPointInformationsMessage()
        {
        }
        public AtlasPointInformationsMessage(AtlasPointsInformations type)
        {
            this.type = type;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            type.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            type = new AtlasPointsInformations();
            type.Deserialize(reader);
		}
	}
}
