using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameFightHumanReadyStateMessage : Message
	{
        public int characterId;
        public bool isReady;

        public const int Id = 740;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightHumanReadyStateMessage()
        {
        }
        public GameFightHumanReadyStateMessage(int characterId, bool isReady)
        {
            this.characterId = characterId;
            this.isReady = isReady;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(characterId);
            writer.WriteBoolean(isReady);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            isReady = reader.ReadBoolean();
		}
	}
}
