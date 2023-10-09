using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Version = Rollback.Protocol.Types.Version;

namespace Rollback.Protocol.Messages
{
    public record IdentificationMessage : Message
    {
        public Version version;
        public string login;
        public string password;
        public bool autoconnect;

        public const int Id = 4;

        public override uint MessageId
        {
            get { return Id; }
        }
        public IdentificationMessage()
        {
        }
        public IdentificationMessage(Version version, string login, string password, bool autoconnect)
        {
            this.version = version;
            this.login = login;
            this.password = password;
            this.autoconnect = autoconnect;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            version.Serialize(writer);
            writer.WriteString(login);
            writer.WriteString(password);
            writer.WriteBoolean(autoconnect);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            version = new Version();
            version.Deserialize(reader);
            login = reader.ReadString();
            password = reader.ReadString();
            autoconnect = reader.ReadBoolean();
        }
    }
}
