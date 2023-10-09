using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record LockableUseCodeMessage : Message
	{
        public string code;

        public const int Id = 5667;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableUseCodeMessage()
        {
        }
        public LockableUseCodeMessage(string code)
        {
            this.code = code;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(code);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            code = reader.ReadString();
		}
	}
}
