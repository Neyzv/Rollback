using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
    public record GameFightEndMessage : Message
    {
        public int duration;
        public short ageBonus;
        public FightResultListEntry[] results;

        public const int Id = 720;
        public override uint MessageId
        {
            get { return Id; }
        }
        public GameFightEndMessage()
        {
        }
        public GameFightEndMessage(int duration, short ageBonus, FightResultListEntry[] results)
        {
            this.duration = duration;
            this.ageBonus = ageBonus;
            this.results = results;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(duration);
            writer.WriteShort(ageBonus);
            writer.WriteUShort((ushort)results.Length);
            foreach (var entry in results)
            {
                writer.WriteShort(entry.TypeId);
                entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            duration = reader.ReadInt();
            if (duration < 0)
                throw new Exception("Forbidden value on duration = " + duration + ", it doesn't respect the following condition : duration < 0");
            ageBonus = reader.ReadShort();
            var limit = reader.ReadUShort();
            results = new FightResultListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                results[i] = (FightResultListEntry)ProtocolManager.Instance.GetType(reader.ReadUShort());
                results[i].Deserialize(reader);
            }
        }
    }
}
