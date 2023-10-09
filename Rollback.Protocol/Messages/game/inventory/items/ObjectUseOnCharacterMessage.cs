using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record ObjectUseOnCharacterMessage : ObjectUseMessage
	{
        public int characterId;

        public new const int Id = 3003;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public ObjectUseOnCharacterMessage()
        {
        }
        public ObjectUseOnCharacterMessage(int objectUID, int characterId) : base(objectUID)
        {
            this.characterId = characterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(characterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
		}
	}
}
