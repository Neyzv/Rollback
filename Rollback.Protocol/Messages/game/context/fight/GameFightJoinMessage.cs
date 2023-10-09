using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Messages
{
    public record GameFightJoinMessage : Message
	{
        public bool canBeCancelled;
        public bool canSayReady;
        public bool isSpectator;
        public bool isFightStarted;
        public int timeMaxBeforeFightStart;
        public sbyte fightType;

        public const int Id = 702;
        public override uint MessageId
        {
        	get { return Id; }
        }
        public GameFightJoinMessage()
        {
        }
        public GameFightJoinMessage(bool canBeCancelled, bool canSayReady, bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart, sbyte fightType)
        {
            this.canBeCancelled = canBeCancelled;
            this.canSayReady = canSayReady;
            this.isSpectator = isSpectator;
            this.isFightStarted = isFightStarted;
            this.timeMaxBeforeFightStart = timeMaxBeforeFightStart;
            this.fightType = fightType;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, canBeCancelled);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, canSayReady);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 2, isSpectator);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 3, isFightStarted);
            writer.WriteByte(flag1);
            writer.WriteInt(timeMaxBeforeFightStart);
            writer.WriteSByte(fightType);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            canBeCancelled = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            canSayReady = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            isSpectator = BigEndianBooleanByteWrapper.GetFlag(flag1, 2);
            isFightStarted = BigEndianBooleanByteWrapper.GetFlag(flag1, 3);
            timeMaxBeforeFightStart = reader.ReadInt();
            if (timeMaxBeforeFightStart < 0)
                throw new Exception("Forbidden value on timeMaxBeforeFightStart = " + timeMaxBeforeFightStart + ", it doesn't respect the following condition : timeMaxBeforeFightStart < 0");
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
		}
	}
}
