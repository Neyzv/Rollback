using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameRolePlayNpcInformations : GameRolePlayActorInformations
    {
        public short npcId;
        public bool sex;
        public short specialArtworkId;
        public bool canGiveQuest;
        public new const short Id = 156;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayNpcInformations()
        {
        }
        public GameRolePlayNpcInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, short npcId, bool sex, short specialArtworkId, bool canGiveQuest) : base(contextualId, look, disposition)
        {
            this.npcId = npcId;
            this.sex = sex;
            this.specialArtworkId = specialArtworkId;
            this.canGiveQuest = canGiveQuest;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(npcId);
            writer.WriteBoolean(sex);
            writer.WriteShort(specialArtworkId);
            writer.WriteBoolean(canGiveQuest);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            npcId = reader.ReadShort();
            if (npcId < 0)
                throw new Exception("Forbidden value on npcId = " + npcId + ", it doesn't respect the following condition : npcId < 0");
            sex = reader.ReadBoolean();
            specialArtworkId = reader.ReadShort();
            if (specialArtworkId < 0)
                throw new Exception("Forbidden value on specialArtworkId = " + specialArtworkId + ", it doesn't respect the following condition : specialArtworkId < 0");
            canGiveQuest = reader.ReadBoolean();
        }
    }
}