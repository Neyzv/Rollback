using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record PrismConquestInformation
    {
        public bool isEntered;
        public bool isInRoom;
        public int subId;
        public sbyte alignment;
        public const short Id = 152;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public PrismConquestInformation()
        {
        }
        public PrismConquestInformation(bool isEntered, bool isInRoom, int subId, sbyte alignment)
        {
            this.isEntered = isEntered;
            this.isInRoom = isInRoom;
            this.subId = subId;
            this.alignment = alignment;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, isEntered);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, isInRoom);
            writer.WriteByte(flag1);
            writer.WriteInt(subId);
            writer.WriteSByte(alignment);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            isEntered = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            isInRoom = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            subId = reader.ReadInt();
            if (subId < 0)
                throw new Exception("Forbidden value on subId = " + subId + ", it doesn't respect the following condition : subId < 0");
            alignment = reader.ReadSByte();
            if (alignment < 0)
                throw new Exception("Forbidden value on alignment = " + alignment + ", it doesn't respect the following condition : alignment < 0");
        }
    }
}