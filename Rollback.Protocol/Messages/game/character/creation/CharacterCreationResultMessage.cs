using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterCreationResultMessage : Message
	{
        public sbyte result;

        public const int Id = 161;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterCreationResultMessage()
        {
        }
        public CharacterCreationResultMessage(sbyte result)
        {
            this.result = result;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(result);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
		}
	}
}
