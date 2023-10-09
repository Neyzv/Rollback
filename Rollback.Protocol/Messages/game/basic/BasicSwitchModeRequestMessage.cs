using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicSwitchModeRequestMessage : Message
	{
        public sbyte mode;

        public const int Id = 6101;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicSwitchModeRequestMessage()
        {
        }
        public BasicSwitchModeRequestMessage(sbyte mode)
        {
            this.mode = mode;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(mode);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            mode = reader.ReadSByte();
		}
	}
}
