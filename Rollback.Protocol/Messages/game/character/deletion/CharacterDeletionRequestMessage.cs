using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterDeletionRequestMessage : Message
	{
        public int characterId;
        public string secretAnswerHash;

        public const int Id = 165;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterDeletionRequestMessage()
        {
        }
        public CharacterDeletionRequestMessage(int characterId, string secretAnswerHash)
        {
            this.characterId = characterId;
            this.secretAnswerHash = secretAnswerHash;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(characterId);
            writer.WriteString(secretAnswerHash);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            secretAnswerHash = reader.ReadString();
		}
	}
}
