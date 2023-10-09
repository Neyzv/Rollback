using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;
using Rollback.Protocol.Types;

namespace Rollback.Protocol.Messages
{
    public record JobCrafterDirectoryEntryMessage : Message
	{
        public JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public JobCrafterDirectoryEntryJobInfo[] jobInfoList;
        public EntityLook playerLook;

        public const int Id = 6044;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public JobCrafterDirectoryEntryMessage()
        {
        }
        public JobCrafterDirectoryEntryMessage(JobCrafterDirectoryEntryPlayerInfo playerInfo, JobCrafterDirectoryEntryJobInfo[] jobInfoList, EntityLook playerLook)
        {
            this.playerInfo = playerInfo;
            this.jobInfoList = jobInfoList;
            this.playerLook = playerLook;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            playerInfo.Serialize(writer);
            writer.WriteUShort((ushort)jobInfoList.Length);
            foreach (var entry in jobInfoList)
            {
                 entry.Serialize(writer);
            }
            playerLook.Serialize(writer);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            jobInfoList = new JobCrafterDirectoryEntryJobInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 jobInfoList[i] = new JobCrafterDirectoryEntryJobInfo();
                 jobInfoList[i].Deserialize(reader);
            }
            playerLook = new EntityLook();
            playerLook.Deserialize(reader);
		}
	}
}
