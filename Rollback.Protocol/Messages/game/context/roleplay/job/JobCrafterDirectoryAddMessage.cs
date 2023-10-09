using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryAddMessage : Message
	{
        public JobCrafterDirectoryListEntry listEntry;

        public const int Id = 5651;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryAddMessage()
        {
        }
        public JobCrafterDirectoryAddMessage(JobCrafterDirectoryListEntry listEntry)
        {
            this.listEntry = listEntry;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            listEntry.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            listEntry = new JobCrafterDirectoryListEntry();
            listEntry.Deserialize(reader);
		}
	}
}
