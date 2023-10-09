using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryDefineSettingsMessage : Message
	{
        public JobCrafterDirectorySettings settings;

        public const int Id = 5649;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryDefineSettingsMessage()
        {
        }
        public JobCrafterDirectoryDefineSettingsMessage(JobCrafterDirectorySettings settings)
        {
            this.settings = settings;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            settings.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            settings = new JobCrafterDirectorySettings();
            settings.Deserialize(reader);
		}
	}
}
