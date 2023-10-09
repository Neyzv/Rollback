using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CompassUpdatePvpSeekMessage : CompassUpdateMessage
	{
        public int memberId;
        public string memberName;

        public new const int Id = 6013;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CompassUpdatePvpSeekMessage()
        {
        }
        public CompassUpdatePvpSeekMessage(sbyte type, short worldX, short worldY, int memberId, string memberName) : base(type, worldX, worldY)
        {
            this.memberId = memberId;
            this.memberName = memberName;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(memberId);
            writer.WriteString(memberName);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            memberId = reader.ReadInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            memberName = reader.ReadString();
		}
	}
}
