using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record GuildInformationsMemberUpdateMessage : Message
	{
        public GuildMember member;

        public const int Id = 5597;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GuildInformationsMemberUpdateMessage()
        {
        }
        public GuildInformationsMemberUpdateMessage(GuildMember member)
        {
            this.member = member;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            member.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            member = new GuildMember();
            member.Deserialize(reader);
		}
	}
}
