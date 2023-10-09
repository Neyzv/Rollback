using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record GameRolePlayGroupMonsterInformations : GameRolePlayActorInformations
    {
        public int mainCreatureGenericId;
        public short mainCreaturelevel;
        public MonsterInGroupInformations[] underlings;
        public short ageBonus;
        public new const short Id = 160;
        public override short TypeId
        {
            get { return Id; }
        }
        public GameRolePlayGroupMonsterInformations()
        {
        }
        public GameRolePlayGroupMonsterInformations(int contextualId, EntityLook look, EntityDispositionInformations disposition, int mainCreatureGenericId, short mainCreaturelevel, MonsterInGroupInformations[] underlings, short ageBonus) : base(contextualId, look, disposition)
        {
            this.mainCreatureGenericId = mainCreatureGenericId;
            this.mainCreaturelevel = mainCreaturelevel;
            this.underlings = underlings;
            this.ageBonus = ageBonus;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(mainCreatureGenericId);
            writer.WriteShort(mainCreaturelevel);
            writer.WriteUShort((ushort)underlings.Length);
            foreach (var entry in underlings)
            {
                entry.Serialize(writer);
            }
            writer.WriteShort(ageBonus);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            mainCreatureGenericId = reader.ReadInt();
            mainCreaturelevel = reader.ReadShort();
            if (mainCreaturelevel < 0)
                throw new Exception("Forbidden value on mainCreaturelevel = " + mainCreaturelevel + ", it doesn't respect the following condition : mainCreaturelevel < 0");
            var limit = reader.ReadUShort();
            underlings = new MonsterInGroupInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                underlings[i] = new MonsterInGroupInformations();
                underlings[i].Deserialize(reader);
            }
            ageBonus = reader.ReadShort();
            if (ageBonus < -1 || ageBonus > 1000)
                throw new Exception("Forbidden value on ageBonus = " + ageBonus + ", it doesn't respect the following condition : ageBonus < -1 || ageBonus > 1000");
        }
    }
}
        