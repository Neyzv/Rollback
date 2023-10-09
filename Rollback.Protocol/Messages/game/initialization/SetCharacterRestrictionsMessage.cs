using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record SetCharacterRestrictionsMessage : Message
	{
        public ActorRestrictionsInformations restrictions;

        public const int Id = 170;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public SetCharacterRestrictionsMessage()
        {
        }
        public SetCharacterRestrictionsMessage(ActorRestrictionsInformations restrictions)
        {
            this.restrictions = restrictions;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            restrictions.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            restrictions = new ActorRestrictionsInformations();
            restrictions.Deserialize(reader);
		}
	}
}
