using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Messages
{
    public record BasicWhoIsMessage : Message
	{
        public bool self;
        public sbyte position;
        public string accountNickname;
        public string characterName;
        public short areaId;

        public const int Id = 180;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public BasicWhoIsMessage()
        {
        }
        public BasicWhoIsMessage(bool self, sbyte position, string accountNickname, string characterName, short areaId)
        {
            this.self = self;
            this.position = position;
            this.accountNickname = accountNickname;
            this.characterName = characterName;
            this.areaId = areaId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteBoolean(self);
            writer.WriteSByte(position);
            writer.WriteString(accountNickname);
            writer.WriteString(characterName);
            writer.WriteShort(areaId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            self = reader.ReadBoolean();
            position = reader.ReadSByte();
            accountNickname = reader.ReadString();
            characterName = reader.ReadString();
            areaId = reader.ReadShort();
		}
	}
}
