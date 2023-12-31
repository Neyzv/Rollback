using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ContactLookRequestByIdMessage : ContactLookRequestMessage
	{
        public int playerId;

        public new const int Id = 5935;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ContactLookRequestByIdMessage()
        {
        }
        public ContactLookRequestByIdMessage(byte requestId, sbyte contactType, int playerId) : base(requestId, contactType)
        {
            this.playerId = playerId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(playerId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
		}
	}
}
