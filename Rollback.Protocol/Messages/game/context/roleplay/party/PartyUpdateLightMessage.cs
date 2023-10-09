using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyUpdateLightMessage : Message
	{
        public int id;
        public int lifePoints;
        public int maxLifePoints;
        public short prospecting;
        public byte regenRate;

        public const int Id = 6054;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyUpdateLightMessage()
        {
        }
        public PartyUpdateLightMessage(int id, int lifePoints, int maxLifePoints, short prospecting, byte regenRate)
        {
            this.id = id;
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.prospecting = prospecting;
            this.regenRate = regenRate;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteShort(prospecting);
            writer.WriteByte(regenRate);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            prospecting = reader.ReadShort();
            if (prospecting < 0)
                throw new Exception("Forbidden value on prospecting = " + prospecting + ", it doesn't respect the following condition : prospecting < 0");
            regenRate = reader.ReadByte();
            if (regenRate < 0 || regenRate > 255)
                throw new Exception("Forbidden value on regenRate = " + regenRate + ", it doesn't respect the following condition : regenRate < 0 || regenRate > 255");
		}
	}
}
