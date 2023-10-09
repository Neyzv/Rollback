using Rollback.Common.IO.Binary;
using Rollback.Common.Network.Protocol;


namespace Rollback.Protocol.Types
{
    public record ObjectEffectDice : ObjectEffect
    {
        public short diceNum;
        public short diceSide;
        public short diceConst;
        public new const short Id = 73;
        public override short TypeId
        {
            get { return Id; }
        }
        public ObjectEffectDice()
        {
        }
        public ObjectEffectDice(short actionId, short diceNum, short diceSide, short diceConst) : base(actionId)
        {
            this.diceNum = diceNum;
            this.diceSide = diceSide;
            this.diceConst = diceConst;
        }
        public override void Serialize(BigEndianWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(diceNum);
            writer.WriteShort(diceSide);
            writer.WriteShort(diceConst);
        }
        public override void Deserialize(BigEndianReader reader)
        {
            base.Deserialize(reader);
            diceNum = reader.ReadShort();
            if (diceNum < 0)
                throw new Exception("Forbidden value on diceNum = " + diceNum + ", it doesn't respect the following condition : diceNum < 0");
            diceSide = reader.ReadShort();
            if (diceSide < 0)
                throw new Exception("Forbidden value on diceSide = " + diceSide + ", it doesn't respect the following condition : diceSide < 0");
            diceConst = reader.ReadShort();
            if (diceConst < 0)
                throw new Exception("Forbidden value on diceConst = " + diceConst + ", it doesn't respect the following condition : diceConst < 0");
        }
    }
}
