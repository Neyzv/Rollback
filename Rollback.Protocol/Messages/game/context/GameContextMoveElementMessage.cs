using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameContextMoveElementMessage : Message
	{
        public EntityMovementInformations movement;

        public const int Id = 253;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextMoveElementMessage()
        {
        }
        public GameContextMoveElementMessage(EntityMovementInformations movement)
        {
            this.movement = movement;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            movement.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            movement = new EntityMovementInformations();
            movement.Deserialize(reader);
		}
	}
}
