using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record PartyKickedByMessage : Message
	{
        public int kickerId;

        public const int Id = 5590;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public PartyKickedByMessage()
        {
        }
        public PartyKickedByMessage(int kickerId)
        {
            this.kickerId = kickerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(kickerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            kickerId = reader.ReadInt();
            if (kickerId < 0)
                throw new Exception("Forbidden value on kickerId = " + kickerId + ", it doesn't respect the following condition : kickerId < 0");
		}
	}
}
