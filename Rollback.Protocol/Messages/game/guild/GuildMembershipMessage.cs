using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GuildMembershipMessage : GuildJoinedMessage
	{
        public new const int Id = 5835;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildMembershipMessage()
        {
        }
        public GuildMembershipMessage(string guildName, GuildEmblem guildEmblem, uint memberRights) : base(guildName, guildEmblem, memberRights)
        {
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
		}
	}
}
