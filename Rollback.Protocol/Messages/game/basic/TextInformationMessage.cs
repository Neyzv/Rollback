using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record TextInformationMessage : Message
	{
        public sbyte msgType;
        public short msgId;
        public string[] parameters;

        public const int Id = 780;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public TextInformationMessage()
        {
        }
        public TextInformationMessage(sbyte msgType, short msgId, string[] parameters)
        {
            this.msgType = msgType;
            this.msgId = msgId;
            this.parameters = parameters;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(msgType);
            writer.WriteShort(msgId);
            writer.WriteUShort((ushort)parameters.Length);
            foreach (var entry in parameters)
            {
                 writer.WriteString(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            msgType = reader.ReadSByte();
            if (msgType < 0)
                throw new Exception("Forbidden value on msgType = " + msgType + ", it doesn't respect the following condition : msgType < 0");
            msgId = reader.ReadShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters[i] = reader.ReadString();
            }
		}
	}
}
