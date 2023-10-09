using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryListMessage : Message
	{
        public JobCrafterDirectoryListEntry[] listEntries;

        public const int Id = 6046;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryListMessage()
        {
        }
        public JobCrafterDirectoryListMessage(JobCrafterDirectoryListEntry[] listEntries)
        {
            this.listEntries = listEntries;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteUShort((ushort)listEntries.Length);
            foreach (var entry in listEntries)
            {
                 entry.Serialize(writer);
            }
        }
        public override void Deserialize(BigEndianReader reader)
        {
            var limit = reader.ReadUShort();
            listEntries = new JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 listEntries[i] = new JobCrafterDirectoryListEntry();
                 listEntries[i].Deserialize(reader);
            }
		}
	}
}
