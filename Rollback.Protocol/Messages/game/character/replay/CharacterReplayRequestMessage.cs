using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterReplayRequestMessage : Message
	{
        public int characterId;

        public const int Id = 167;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterReplayRequestMessage()
        {
        }
        public CharacterReplayRequestMessage(int characterId)
        {
            this.characterId = characterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(characterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
		}
	}
}
