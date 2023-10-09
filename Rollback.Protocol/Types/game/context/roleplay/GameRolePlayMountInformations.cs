using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameRolePlayMountInformations : GameRolePlayNamedActorInformations
    {
        public string ownerName;
        public byte level;
        public new const short Id = 180;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayMountInformations()
        {
        }
        public GameRolePlayMountInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, string name, string ownerName, byte level) : base(contextualId, look, disposition, name)
        {
            this.ownerName = ownerName;
            this.level = level;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteString(ownerName);
            writer.WriteByte(level);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            ownerName = reader.ReadString();
            level = reader.ReadByte();
            if (level < 0 || level > 255)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0 || level > 255");
        }
    }
}

