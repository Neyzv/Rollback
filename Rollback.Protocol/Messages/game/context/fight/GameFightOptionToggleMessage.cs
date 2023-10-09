using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameFightOptionToggleMessage : Message
	{
        public sbyte option;

        public const int Id = 707;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightOptionToggleMessage()
        {
        }
        public GameFightOptionToggleMessage(sbyte option)
        {
            this.option = option;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(option);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            option = reader.ReadSByte();
            if (option < 0)
                throw new Exception("Forbidden value on option = " + option + ", it doesn't respect the following condition : option < 0");
		}
	}
}
