using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CompassUpdatePartyMemberMessage : CompassUpdateMessage
	{
        public int memberId;

        public new const int Id = 5589;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CompassUpdatePartyMemberMessage()
        {
        }
        public CompassUpdatePartyMemberMessage(sbyte type, short worldX, short worldY, int memberId) : base(type, worldX, worldY)
        {
            this.memberId = memberId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(memberId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            memberId = reader.ReadInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
		}
	}
}
