using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record NpcGenericActionRequestMessage : Message
	{
        public int npcId;
        public sbyte npcActionId;

        public const int Id = 5898;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public NpcGenericActionRequestMessage()
        {
        }
        public NpcGenericActionRequestMessage(int npcId, sbyte npcActionId)
        {
            this.npcId = npcId;
            this.npcActionId = npcActionId;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(npcId);
            writer.WriteSByte(npcActionId);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            npcId = reader.ReadInt();
            npcActionId = reader.ReadSByte();
            if (npcActionId < 0)
                throw new Exception("Forbidden value on npcActionId = " + npcActionId + ", it doesn't respect the following condition : npcActionId < 0");
		}
	}
}
