using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record JobCrafterDirectoryListEntry
    {
        public JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public JobCrafterDirectoryEntryJobInfo jobInfo;
        public const short Id = 196;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public JobCrafterDirectoryListEntry()
        {
        }
        public JobCrafterDirectoryListEntry(JobCrafterDirectoryEntryPlayerInfo playerInfo, JobCrafterDirectoryEntryJobInfo jobInfo)
        {
            this.playerInfo = playerInfo;
            this.jobInfo = jobInfo;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            playerInfo.Serialize(writer);
            jobInfo.Serialize(writer);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            jobInfo = new JobCrafterDirectoryEntryJobInfo();
            jobInfo.Deserialize(reader);
        }
    }
}

