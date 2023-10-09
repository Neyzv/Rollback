using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record NicknameChoiceRequestMessage : Message
	{
        public string nickname;
        public const int Id = 5639;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NicknameChoiceRequestMessage()
        {
        }
        public NicknameChoiceRequestMessage(string nickname)
        {
            this.nickname = nickname;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteString(nickname);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            nickname = reader.ReadString();
		}
	}
}
