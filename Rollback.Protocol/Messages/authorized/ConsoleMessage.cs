using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ConsoleMessage : Message
	{
        public sbyte type;
        public string content;

        public const int Id = 75;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ConsoleMessage()
        {
        }
        public ConsoleMessage(sbyte type, string content)
        {
            this.type = type;
            this.content = content;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(type);
            writer.WriteString(content);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            content = reader.ReadString();
		}
	}
}
