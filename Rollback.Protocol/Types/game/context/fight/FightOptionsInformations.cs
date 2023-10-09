using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record FightOptionsInformations
    {
        public bool isSecret;
        public bool isRestrictedToPartyOnly;
        public bool isClosed;
        public bool isAskingForHelp;
        public const short Id = 20;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public FightOptionsInformations()
        {
        }
        public FightOptionsInformations(bool isSecret, bool isRestrictedToPartyOnly, bool isClosed, bool isAskingForHelp)
        {
            this.isSecret = isSecret;
            this.isRestrictedToPartyOnly = isRestrictedToPartyOnly;
            this.isClosed = isClosed;
            this.isAskingForHelp = isAskingForHelp;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, isSecret);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, isRestrictedToPartyOnly);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 2, isClosed);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 3, isAskingForHelp);
            writer.WriteByte(flag1);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            isSecret = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            isRestrictedToPartyOnly = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            isClosed = BigEndianBooleanByteWrapper.GetFlag(flag1, 2);
            isAskingForHelp = BigEndianBooleanByteWrapper.GetFlag(flag1, 3);
        }
    }
}