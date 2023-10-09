using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameContextKickMessage : Message
	{
        public int targetId;

        public const int Id = 6081;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextKickMessage()
        {
        }
        public GameContextKickMessage(int targetId)
        {
            this.targetId = targetId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(targetId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            targetId = reader.ReadInt();
		}
	}
}
