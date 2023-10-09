using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;


namespace Rollback.Protocol.Messages
{
	public record ContactLookMessage : Message
	{
        public int requestId;
        public string playerName;
        public int playerId;
        public EntityLook look;

        public const int Id = 5934;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ContactLookMessage()
        {
        }
        public ContactLookMessage(int requestId, string playerName, int playerId, EntityLook look)
        {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(requestId);
            writer.WriteString(playerName);
            writer.WriteInt(playerId);
            look.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            playerName = reader.ReadString();
            playerId = reader.ReadInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            look = new EntityLook();
            look.Deserialize(reader);
		}
	}
}
