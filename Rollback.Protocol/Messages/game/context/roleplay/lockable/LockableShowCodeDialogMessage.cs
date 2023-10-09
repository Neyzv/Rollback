using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record LockableShowCodeDialogMessage : Message
	{
        public bool changeOrUse;
        public sbyte codeSize;

        public const int Id = 5740;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public LockableShowCodeDialogMessage()
        {
        }
        public LockableShowCodeDialogMessage(bool changeOrUse, sbyte codeSize)
        {
            this.changeOrUse = changeOrUse;
            this.codeSize = codeSize;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(changeOrUse);
            writer.WriteSByte(codeSize);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            changeOrUse = reader.ReadBoolean();
            codeSize = reader.ReadSByte();
            if (codeSize < 0)
                throw new Exception("Forbidden value on codeSize = " + codeSize + ", it doesn't respect the following condition : codeSize < 0");
		}
	}
}
