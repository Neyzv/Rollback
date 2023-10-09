using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterNameSuggestionFailureMessage : Message
	{
        public sbyte reason;

        public const int Id = 164;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterNameSuggestionFailureMessage()
        {
        }
        public CharacterNameSuggestionFailureMessage(sbyte reason)
        {
            this.reason = reason;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(reason);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
		}
	}
}
