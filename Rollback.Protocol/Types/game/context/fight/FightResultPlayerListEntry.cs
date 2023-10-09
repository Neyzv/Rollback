using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record FightResultPlayerListEntry : FightResultFighterListEntry
    {
        public byte level;
        public FightResultAdditionalData[] additional;
        public new const short Id = 24;
        public override short TypeId
        {
            get { return Id; }
        }
        public FightResultPlayerListEntry()
        {
        }
        public FightResultPlayerListEntry(short outcome, Types.FightLoot rewards, int id, bool alive, byte level, FightResultAdditionalData[] additional) : base(outcome, rewards, id, alive)
        {
            this.level = level;
            this.additional = additional;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(level);
            writer.WriteUShort((ushort)additional.Length);
            foreach (var entry in additional)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            var limit = reader.ReadUShort();
            additional = new Types.FightResultAdditionalData[limit];
            for (int i = 0; i < limit; i++)
            {
                additional[i] = (FightResultAdditionalData)ProtocolManager.Instance.GetType(reader.ReadUShort());
                additional[i].Deserialize(reader);
            }
        }
    }
}
