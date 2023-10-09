using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameContextCreateMessage : Message
	{
        public sbyte context;

        public const int Id = 200;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameContextCreateMessage()
        {
        }
        public GameContextCreateMessage(sbyte context)
        {
            this.context = context;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(context);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            context = reader.ReadSByte();
            if (context < 0)
                throw new Exception("Forbidden value on context = " + context + ", it doesn't respect the following condition : context < 0");
		}
	}
}
