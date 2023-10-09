using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage
	{
        public int playerId;
        public short[] skills;

        public new const int Id = 5747;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobMultiCraftAvailableSkillsMessage()
        {
        }
        public JobMultiCraftAvailableSkillsMessage(bool enabled, int playerId, short[] skills) : base(enabled)
        {
            this.playerId = playerId;
            this.skills = skills;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(playerId);
            writer.WriteUShort((ushort)skills.Length);
            foreach (var entry in skills)
            {
                 writer.WriteShort(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            var limit = reader.ReadUShort();
            skills = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 skills[i] = reader.ReadShort();
            }
		}
	}
}
