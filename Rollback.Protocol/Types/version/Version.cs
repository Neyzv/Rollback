using System.Text.Json.Serialization;
using Rollback.Common.IO.Binary;

namespace Rollback.Protocol.Types
{
    public record Version
    {
        public sbyte Major { get; set; }
        public sbyte Minor { get; set; }
        public sbyte Revision { get; set; }
        public sbyte BuildType { get; set; }
        public const short Id = 11;
        [JsonIgnore]
        public virtual short TypeId
        {
            get { return Id; }
        }
        public Version()
        {
        }
        public Version(sbyte major, sbyte minor, sbyte revision, sbyte buildType)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            BuildType = buildType;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            writer.WriteSByte(Major);
            writer.WriteSByte(Minor);
            writer.WriteSByte(Revision);
            writer.WriteSByte(BuildType);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            Major = reader.ReadSByte();
            if (Major < 0)
                throw new Exception("Forbidden value on major = " + Major + ", it doesn't respect the following condition : major < 0");
            Minor = reader.ReadSByte();
            if (Minor < 0)
                throw new Exception("Forbidden value on minor = " + Minor + ", it doesn't respect the following condition : minor < 0");
            Revision = reader.ReadSByte();
            if (Revision < 0)
                throw new Exception("Forbidden value on revision = " + Revision + ", it doesn't respect the following condition : revision < 0");
            BuildType = reader.ReadSByte();
            if (BuildType < 0)
                throw new Exception("Forbidden value on buildType = " + BuildType + ", it doesn't respect the following condition : buildType < 0");
        }
    }
}