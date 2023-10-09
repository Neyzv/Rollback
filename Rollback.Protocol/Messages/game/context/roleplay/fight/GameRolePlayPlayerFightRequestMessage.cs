using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameRolePlayPlayerFightRequestMessage : Message
	{
        public int targetId;
        public bool friendly;

        public const int Id = 5731;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameRolePlayPlayerFightRequestMessage()
        {
        }
        public GameRolePlayPlayerFightRequestMessage(int targetId, bool friendly)
        {
            this.targetId = targetId;
            this.friendly = friendly;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(targetId);
            writer.WriteBoolean(friendly);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            targetId = reader.ReadInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
            friendly = reader.ReadBoolean();
		}
	}
}
