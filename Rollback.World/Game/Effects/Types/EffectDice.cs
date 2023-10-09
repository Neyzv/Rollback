using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rnd = System.Random;

namespace Rollback.World.Game.Effects.Types
{
    public sealed class EffectDice : EffectInteger
    {
        public override byte SerializationId =>
            4;

        public short DiceNum { get; set; }

        public short DiceFace { get; set; }

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectDice((short)Id, DiceNum, DiceFace, Value);

        public override object[] Values =>
            new object[] { DiceNum, DiceFace, Value };

        public override EffectBase GenerateEffect(EffectGenerationType type, EffectGenerationContext context)
        {
            EffectBase effect;
            if (context is EffectGenerationContext.Spell || !EffectManager.IsDiceEffect(Id))
            {
                if (DiceNum is 0 && DiceFace is 0)
                    effect = new EffectInteger(this, Value);
                else if (DiceNum > DiceFace)
                    effect = new EffectInteger(this, DiceNum);
                else if (type is EffectGenerationType.MaxEffects)
                    effect = new EffectInteger(this, Record?.Operator is "-" ? DiceNum : DiceFace);
                else if (type is EffectGenerationType.MinEffects)
                    effect = new EffectInteger(this, Record?.Operator is "-" ? DiceFace : DiceNum);
                else
                    effect = new EffectInteger(this, (short)Rnd.Shared.Next(DiceNum, DiceFace + 1));
            }
            else
                effect = Clone();

            return effect;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);

            writer.WriteShort(DiceNum);
            writer.WriteShort(DiceFace);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);

            DiceNum = reader.ReadShort();
            DiceFace = reader.ReadShort();
        }

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), DiceFace, DiceNum);

        public override bool Equals(object? obj) =>
            obj is EffectDice && base.Equals(obj);

        public static bool operator ==(EffectDice left, EffectDice right) =>
            left.Equals(right);

        public static bool operator !=(EffectDice left, EffectDice right) =>
            !left.Equals(right);
    }
}
