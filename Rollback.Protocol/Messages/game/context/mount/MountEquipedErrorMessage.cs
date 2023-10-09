using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record MountEquipedErrorMessage : Message
	{
        public sbyte errorType;

        public const int Id = 5963;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public MountEquipedErrorMessage()
        {
        }
        public MountEquipedErrorMessage(sbyte errorType)
        {
            this.errorType = errorType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(errorType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            errorType = reader.ReadSByte();
            if (errorType < 0)
                throw new Exception("Forbidden value on errorType = " + errorType + ", it doesn't respect the following condition : errorType < 0");
		}
	}
}
