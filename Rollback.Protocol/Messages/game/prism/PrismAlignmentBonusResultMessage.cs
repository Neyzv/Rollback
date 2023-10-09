using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record PrismAlignmentBonusResultMessage : Message
	{
        public AlignmentBonusInformations alignmentBonus;

        public const int Id = 5842;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PrismAlignmentBonusResultMessage()
        {
        }
        public PrismAlignmentBonusResultMessage(AlignmentBonusInformations alignmentBonus)
        {
            this.alignmentBonus = alignmentBonus;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            alignmentBonus.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            alignmentBonus = new AlignmentBonusInformations();
            alignmentBonus.Deserialize(reader);
		}
	}
}
