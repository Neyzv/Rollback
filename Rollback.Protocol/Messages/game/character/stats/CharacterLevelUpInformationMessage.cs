using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record CharacterLevelUpInformationMessage : CharacterLevelUpMessage
	{
        public string name;
        public int id;
        public sbyte relationType;

        public new const int Id = 6076;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public CharacterLevelUpInformationMessage()
        {
        }
        public CharacterLevelUpInformationMessage(byte newLevel, string name, int id, sbyte relationType) : base(newLevel)
        {
            this.name = name;
            this.id = id;
            this.relationType = relationType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(name);
            writer.WriteInt(id);
            writer.WriteSByte(relationType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            name = reader.ReadString();
            id = reader.ReadInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            relationType = reader.ReadSByte();
		}
	}
}
