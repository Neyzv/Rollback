using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GuildInformationsMembersMessage : Message
	{
        public GuildMember[] members;

        public const int Id = 5558;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInformationsMembersMessage()
        {
        }
        public GuildInformationsMembersMessage(GuildMember[] members)
        {
            this.members = members;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)members.Length);
            foreach (var entry in members)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            members = new GuildMember[limit];
            for (int i = 0; i < limit; i++)
            {
                 members[i] = new GuildMember();
                 members[i].Deserialize(reader);
            }
		}
	}
}
