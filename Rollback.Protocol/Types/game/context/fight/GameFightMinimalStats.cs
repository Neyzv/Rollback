using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameFightMinimalStats
    {
        public int lifePoints;
        public int maxLifePoints;
        public short actionPoints;
        public short movementPoints;
        public int summoner;
        public short neutralElementResistPercent;
        public short earthElementResistPercent;
        public short waterElementResistPercent;
        public short airElementResistPercent;
        public short fireElementResistPercent;
        public short dodgePALostProbability;
        public short dodgePMLostProbability;
        public sbyte invisibilityState;
        public const short Id = 31;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public GameFightMinimalStats()
        {
        }
        public GameFightMinimalStats(int lifePoints, int maxLifePoints, short actionPoints, short movementPoints, int summoner, short neutralElementResistPercent, short earthElementResistPercent, short waterElementResistPercent, short airElementResistPercent, short fireElementResistPercent, short dodgePALostProbability, short dodgePMLostProbability, sbyte invisibilityState)
        {
            this.lifePoints = lifePoints;
            this.maxLifePoints = maxLifePoints;
            this.actionPoints = actionPoints;
            this.movementPoints = movementPoints;
            this.summoner = summoner;
            this.neutralElementResistPercent = neutralElementResistPercent;
            this.earthElementResistPercent = earthElementResistPercent;
            this.waterElementResistPercent = waterElementResistPercent;
            this.airElementResistPercent = airElementResistPercent;
            this.fireElementResistPercent = fireElementResistPercent;
            this.dodgePALostProbability = dodgePALostProbability;
            this.dodgePMLostProbability = dodgePMLostProbability;
            this.invisibilityState = invisibilityState;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(lifePoints);
            writer.WriteInt(maxLifePoints);
            writer.WriteShort(actionPoints);
            writer.WriteShort(movementPoints);
            writer.WriteInt(summoner);
            writer.WriteShort(neutralElementResistPercent);
            writer.WriteShort(earthElementResistPercent);
            writer.WriteShort(waterElementResistPercent);
            writer.WriteShort(airElementResistPercent);
            writer.WriteShort(fireElementResistPercent);
            writer.WriteShort(dodgePALostProbability);
            writer.WriteShort(dodgePMLostProbability);
            writer.WriteSByte(invisibilityState);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            lifePoints = reader.ReadInt();
            if (lifePoints < 0)
                throw new Exception("Forbidden value on lifePoints = " + lifePoints + ", it doesn't respect the following condition : lifePoints < 0");
            maxLifePoints = reader.ReadInt();
            if (maxLifePoints < 0)
                throw new Exception("Forbidden value on maxLifePoints = " + maxLifePoints + ", it doesn't respect the following condition : maxLifePoints < 0");
            actionPoints = reader.ReadShort();
            if (actionPoints < 0)
                throw new Exception("Forbidden value on actionPoints = " + actionPoints + ", it doesn't respect the following condition : actionPoints < 0");
            movementPoints = reader.ReadShort();
            if (movementPoints < 0)
                throw new Exception("Forbidden value on movementPoints = " + movementPoints + ", it doesn't respect the following condition : movementPoints < 0");
            summoner = reader.ReadInt();
            neutralElementResistPercent = reader.ReadShort();
            earthElementResistPercent = reader.ReadShort();
            waterElementResistPercent = reader.ReadShort();
            airElementResistPercent = reader.ReadShort();
            fireElementResistPercent = reader.ReadShort();
            dodgePALostProbability = reader.ReadShort();
            if (dodgePALostProbability < 0)
                throw new Exception("Forbidden value on dodgePALostProbability = " + dodgePALostProbability + ", it doesn't respect the following condition : dodgePALostProbability < 0");
            dodgePMLostProbability = reader.ReadShort();
            if (dodgePMLostProbability < 0)
                throw new Exception("Forbidden value on dodgePMLostProbability = " + dodgePMLostProbability + ", it doesn't respect the following condition : dodgePMLostProbability < 0");
            invisibilityState = reader.ReadSByte();
        }
    }
}

