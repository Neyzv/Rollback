using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GameContextMoveMultipleElementsMessage : Message
	{
        public EntityMovementInformations[] movements;

        public const int Id = 254;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextMoveMultipleElementsMessage()
        {
        }
        public GameContextMoveMultipleElementsMessage(EntityMovementInformations[] movements)
        {
            this.movements = movements;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)movements.Length);
            foreach (var entry in movements)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            movements = new EntityMovementInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 movements[i] = new EntityMovementInformations();
                 movements[i].Deserialize(reader);
            }
		}
	}
}
