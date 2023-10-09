using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record CharacterStatsListMessage : Message
	{
        public CharacterCharacteristicsInformations stats;

        public const int Id = 500;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterStatsListMessage()
        {
        }
        public CharacterStatsListMessage(CharacterCharacteristicsInformations stats)
        {
            this.stats = stats;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            stats.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            stats = new CharacterCharacteristicsInformations();
            stats.Deserialize(reader);
		}
	}
}
