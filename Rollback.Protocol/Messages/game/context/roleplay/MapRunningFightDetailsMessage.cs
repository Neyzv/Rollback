using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MapRunningFightDetailsMessage : Message
	{
        public int fightId;
        public string[] names;
        public short[] levels;
        public sbyte teamSwap;
        public bool[] alives;

        public const int Id = 5751;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MapRunningFightDetailsMessage()
        {
        }
        public MapRunningFightDetailsMessage(int fightId, string[] names, short[] levels, sbyte teamSwap, bool[] alives)
        {
            this.fightId = fightId;
            this.names = names;
            this.levels = levels;
            this.teamSwap = teamSwap;
            this.alives = alives;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteUShort((ushort)names.Length);
            foreach (var entry in names)
            {
                 writer.WriteString(entry);
            }
            writer.WriteUShort((ushort)levels.Length);
            foreach (var entry in levels)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteSByte(teamSwap);
            writer.WriteUShort((ushort)alives.Length);
            foreach (var entry in alives)
            {
                 writer.WriteBoolean(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            names = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 names[i] = reader.ReadString();
            }
            limit = reader.ReadUShort();
            levels = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 levels[i] = reader.ReadShort();
            }
            teamSwap = reader.ReadSByte();
            if (teamSwap < 0)
                throw new Exception("Forbidden value on teamSwap = " + teamSwap + ", it doesn't respect the following condition : teamSwap < 0");
            limit = reader.ReadUShort();
            alives = new bool[limit];
            for (int i = 0; i < limit; i++)
            {
                 alives[i] = reader.ReadBoolean();
            }
		}
	}
}
