using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LivingObjectMessageRequestMessage : Message
	{
        public short msgId;
        public string[] parameters;
        public uint livingObject;

        public const int Id = 6066;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LivingObjectMessageRequestMessage()
        {
        }
        public LivingObjectMessageRequestMessage(short msgId, string[] parameters, uint livingObject)
        {
            this.msgId = msgId;
            this.parameters = parameters;
            this.livingObject = livingObject;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(msgId);
            writer.WriteUShort((ushort)parameters.Length);
            foreach (var entry in parameters)
            {
                 writer.WriteString(entry);
            }
            writer.WriteUInt(livingObject);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            msgId = reader.ReadShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters[i] = reader.ReadString();
            }
            livingObject = reader.ReadUInt();
            if (livingObject < 0 || livingObject > 4294967295)
                throw new Exception("Forbidden value on livingObject = " + livingObject + ", it doesn't respect the following condition : livingObject < 0 || livingObject > 4294967295");
		}
	}
}
