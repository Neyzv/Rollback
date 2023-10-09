using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;

namespace Rollback.Protocol.Types
{
    public record ActorRestrictionsInformations
    {
        public bool cantBeAggressed;
        public bool cantBeChallenged;
        public bool cantTrade;
        public bool cantBeAttackedByMutant;
        public bool cantRun;
        public bool forceSlowWalk;
        public bool cantMinimize;
        public bool cantMove;
        public bool cantAggress;
        public bool cantChallenge;
        public bool cantExchange;
        public bool cantAttack;
        public bool cantChat;
        public bool cantBeMerchant;
        public bool cantUseObject;
        public bool cantUseTaxCollector;
        public bool cantUseInteractive;
        public bool cantSpeakToNPC;
        public bool cantChangeZone;
        public bool cantAttackMonster;
        public bool cantWalk8Directions;
        public const short Id = 204;
        public virtual short TypeId
        {
            get { return Id; }
        }
        public ActorRestrictionsInformations()
        {
        }
        public ActorRestrictionsInformations(bool cantBeAggressed, bool cantBeChallenged, bool cantTrade, bool cantBeAttackedByMutant, bool cantRun, bool forceSlowWalk, bool cantMinimize, bool cantMove, bool cantAggress, bool cantChallenge, bool cantExchange, bool cantAttack, bool cantChat, bool cantBeMerchant, bool cantUseObject, bool cantUseTaxCollector, bool cantUseInteractive, bool cantSpeakToNPC, bool cantChangeZone, bool cantAttackMonster, bool cantWalk8Directions)
        {
            this.cantBeAggressed = cantBeAggressed;
            this.cantBeChallenged = cantBeChallenged;
            this.cantTrade = cantTrade;
            this.cantBeAttackedByMutant = cantBeAttackedByMutant;
            this.cantRun = cantRun;
            this.forceSlowWalk = forceSlowWalk;
            this.cantMinimize = cantMinimize;
            this.cantMove = cantMove;
            this.cantAggress = cantAggress;
            this.cantChallenge = cantChallenge;
            this.cantExchange = cantExchange;
            this.cantAttack = cantAttack;
            this.cantChat = cantChat;
            this.cantBeMerchant = cantBeMerchant;
            this.cantUseObject = cantUseObject;
            this.cantUseTaxCollector = cantUseTaxCollector;
            this.cantUseInteractive = cantUseInteractive;
            this.cantSpeakToNPC = cantSpeakToNPC;
            this.cantChangeZone = cantChangeZone;
            this.cantAttackMonster = cantAttackMonster;
            this.cantWalk8Directions = cantWalk8Directions;
        }
        public virtual void Serialize(BigEndianWriter writer)
        {
            byte flag1 = 0;
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 0, cantBeAggressed);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 1, cantBeChallenged);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 2, cantTrade);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 3, cantBeAttackedByMutant);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 4, cantRun);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 5, forceSlowWalk);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 6, cantMinimize);
            flag1 = BigEndianBooleanByteWrapper.SetFlag(flag1, 7, cantMove);
            writer.WriteByte(flag1);
            byte flag2 = 0;
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 0, cantAggress);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 1, cantChallenge);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 2, cantExchange);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 3, cantAttack);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 4, cantChat);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 5, cantBeMerchant);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 6, cantUseObject);
            flag2 = BigEndianBooleanByteWrapper.SetFlag(flag2, 7, cantUseTaxCollector);
            writer.WriteByte(flag2);
            byte flag3 = 0;
            flag3 = BigEndianBooleanByteWrapper.SetFlag(flag3, 0, cantUseInteractive);
            flag3 = BigEndianBooleanByteWrapper.SetFlag(flag3, 1, cantSpeakToNPC);
            flag3 = BigEndianBooleanByteWrapper.SetFlag(flag3, 2, cantChangeZone);
            flag3 = BigEndianBooleanByteWrapper.SetFlag(flag3, 3, cantAttackMonster);
            flag3 = BigEndianBooleanByteWrapper.SetFlag(flag3, 4, cantWalk8Directions);
            writer.WriteByte(flag3);
        }
        public virtual void Deserialize(BigEndianReader reader)
        {
            byte flag1 = reader.ReadByte();
            cantBeAggressed = BigEndianBooleanByteWrapper.GetFlag(flag1, 0);
            cantBeChallenged = BigEndianBooleanByteWrapper.GetFlag(flag1, 1);
            cantTrade = BigEndianBooleanByteWrapper.GetFlag(flag1, 2);
            cantBeAttackedByMutant = BigEndianBooleanByteWrapper.GetFlag(flag1, 3);
            cantRun = BigEndianBooleanByteWrapper.GetFlag(flag1, 4);
            forceSlowWalk = BigEndianBooleanByteWrapper.GetFlag(flag1, 5);
            cantMinimize = BigEndianBooleanByteWrapper.GetFlag(flag1, 6);
            cantMove = BigEndianBooleanByteWrapper.GetFlag(flag1, 7);
            byte flag2 = reader.ReadByte();
            cantAggress = BigEndianBooleanByteWrapper.GetFlag(flag2, 0);
            cantChallenge = BigEndianBooleanByteWrapper.GetFlag(flag2, 1);
            cantExchange = BigEndianBooleanByteWrapper.GetFlag(flag2, 2);
            cantAttack = BigEndianBooleanByteWrapper.GetFlag(flag2, 3);
            cantChat = BigEndianBooleanByteWrapper.GetFlag(flag2, 4);
            cantBeMerchant = BigEndianBooleanByteWrapper.GetFlag(flag2, 5);
            cantUseObject = BigEndianBooleanByteWrapper.GetFlag(flag2, 6);
            cantUseTaxCollector = BigEndianBooleanByteWrapper.GetFlag(flag2, 7);
            byte flag3 = reader.ReadByte();
            cantUseInteractive = BigEndianBooleanByteWrapper.GetFlag(flag3, 0);
            cantSpeakToNPC = BigEndianBooleanByteWrapper.GetFlag(flag3, 1);
            cantChangeZone = BigEndianBooleanByteWrapper.GetFlag(flag3, 2);
            cantAttackMonster = BigEndianBooleanByteWrapper.GetFlag(flag3, 3);
            cantWalk8Directions = BigEndianBooleanByteWrapper.GetFlag(flag3, 4);
        }
    }
}
