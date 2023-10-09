using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record NotificationByServerMessage : Message
	{
        public ushort id;
        public string[] parameters;

        public const int Id = 6103;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NotificationByServerMessage()
        {
        }
        public NotificationByServerMessage(ushort id, string[] parameters)
        {
            this.id = id;
            this.parameters = parameters;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort(id);
            writer.WriteUShort((ushort)parameters.Length);
            foreach (var entry in parameters)
            {
                 writer.WriteString(entry);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            id = reader.ReadUShort();
            if (id < 0 || id > 65535)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0 || id > 65535");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 parameters[i] = reader.ReadString();
            }
		}
	}
}
