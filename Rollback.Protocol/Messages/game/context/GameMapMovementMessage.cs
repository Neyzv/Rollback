using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record GameMapMovementMessage : Message
	{
        public int actorId;
        public short[] keyMovements;

        public const int Id = 951;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameMapMovementMessage()
        {
        }
        public GameMapMovementMessage(int actorId, short[] keyMovements)
        {
            this.actorId = actorId;
            this.keyMovements = keyMovements;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(actorId);
            writer.WriteUShort((ushort)keyMovements.Length);
            foreach (var entry in keyMovements)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            actorId = reader.ReadInt();
            var limit = reader.ReadUShort();
            keyMovements = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 keyMovements[i] = reader.ReadShort();
            }
		}
	}
}
