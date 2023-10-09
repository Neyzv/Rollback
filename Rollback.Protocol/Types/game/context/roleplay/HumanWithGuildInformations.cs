using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record HumanWithGuildInformations : HumanInformations
    {
        public GuildInformations guildInformations;
        public new const short Id = 153;
        public override short TypeId
        {
            get { return Id; }
        }
        public HumanWithGuildInformations()
        {
        }
        public HumanWithGuildInformations(EntityLook[] followingCharactersLook, sbyte emoteId, ushort emoteEndTime, ActorRestrictionsInformations restrictions, short titleId, GuildInformations guildInformations) : base(followingCharactersLook, emoteId, emoteEndTime, restrictions, titleId)
        {
            this.guildInformations = guildInformations;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            guildInformations.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            guildInformations = new GuildInformations();
            guildInformations.Deserialize(reader);
        }
    }
}
