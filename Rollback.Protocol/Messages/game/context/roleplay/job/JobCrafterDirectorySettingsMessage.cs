using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectorySettingsMessage : Message
	{
        public JobCrafterDirectorySettings[] craftersSettings;

        public const int Id = 5652;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectorySettingsMessage()
        {
        }
        public JobCrafterDirectorySettingsMessage(JobCrafterDirectorySettings[] craftersSettings)
        {
            this.craftersSettings = craftersSettings;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)craftersSettings.Length);
            foreach (var entry in craftersSettings)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            craftersSettings = new JobCrafterDirectorySettings[limit];
            for (int i = 0; i < limit; i++)
            {
                 craftersSettings[i] = new JobCrafterDirectorySettings();
                 craftersSettings[i].Deserialize(reader);
            }
		}
	}
}
