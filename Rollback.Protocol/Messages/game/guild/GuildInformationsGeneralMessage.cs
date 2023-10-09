using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GuildInformationsGeneralMessage : Message
    {
        public bool enabled;
        public byte level;
        public double expLevelFloor;
        public double experience;
        public double expNextLevelFloor;

        public const int Id = 5557;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GuildInformationsGeneralMessage()
        {
        }
        public GuildInformationsGeneralMessage(bool enabled, byte level, double expLevelFloor, double experience, double expNextLevelFloor)
        {
            this.enabled = enabled;
            this.level = level;
            this.expLevelFloor = expLevelFloor;
            this.experience = experience;
            this.expNextLevelFloor = expNextLevelFloor;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(enabled);
            writer.WriteByte(level);
            writer.WriteDouble(expLevelFloor);
            writer.WriteDouble(experience);
            writer.WriteDouble(expNextLevelFloor);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            enabled = reader.ReadBoolean();
            level = reader.ReadByte();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            expLevelFloor = reader.ReadDouble();
            if (expLevelFloor < 0)
                throw new Exception("Forbidden value on expLevelFloor = " + expLevelFloor + ", it doesn't respect the following condition : expLevelFloor < 0");
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            expNextLevelFloor = reader.ReadDouble();
            if (expNextLevelFloor < 0)
                throw new Exception("Forbidden value on expNextLevelFloor = " + expNextLevelFloor + ", it doesn't respect the following condition : expNextLevelFloor < 0");
        }
    }
}
