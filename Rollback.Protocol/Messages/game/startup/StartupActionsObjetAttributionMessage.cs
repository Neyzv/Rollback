using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record StartupActionsObjetAttributionMessage : Message
	{
        public short actionId;
        public int characterId;

        public const int Id = 1303;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public StartupActionsObjetAttributionMessage()
        {
        }
        public StartupActionsObjetAttributionMessage(short actionId, int characterId)
        {
            this.actionId = actionId;
            this.characterId = characterId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteShort(actionId);
            writer.WriteInt(characterId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            actionId = reader.ReadShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
		}
	}
}
