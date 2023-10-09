using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record EmotePlayMassiveMessage : EmotePlayAbstractMessage
	{
        public int[] actorIds;

        public new const int Id = 5691;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public EmotePlayMassiveMessage()
        {
        }
        public EmotePlayMassiveMessage(sbyte emoteId, byte duration, int[] actorIds) : base(emoteId, duration)
        {
            this.actorIds = actorIds;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUShort((ushort)actorIds.Length);
            foreach (var entry in actorIds)
            {
                 writer.WriteInt(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            var limit = reader.ReadUShort();
            actorIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 actorIds[i] = reader.ReadInt();
            }
		}
	}
}
