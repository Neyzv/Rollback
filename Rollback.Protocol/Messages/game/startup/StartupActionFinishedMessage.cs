using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record StartupActionFinishedMessage : Message
	{
        public bool success;
        public bool automaticAction;
        public int actionId;

        public const int Id = 1304;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StartupActionFinishedMessage()
        {
        }
        public StartupActionFinishedMessage(bool success, bool automaticAction, int actionId)
        {
            this.success = success;
            this.automaticAction = automaticAction;
            this.actionId = actionId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, success);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, automaticAction);
            writer.WriteByte(flag1);
            writer.WriteInt(actionId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            success = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            automaticAction = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
		}
	}
}
