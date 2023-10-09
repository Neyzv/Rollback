using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record CharacterSelectedSuccessMessage : Message
	{
        public CharacterBaseInformations infos;

        public const int Id = 153;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterSelectedSuccessMessage()
        {
        }
        public CharacterSelectedSuccessMessage(CharacterBaseInformations infos)
        {
            this.infos = infos;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            infos.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            infos = new CharacterBaseInformations();
            infos.Deserialize(reader);
		}
	}
}
