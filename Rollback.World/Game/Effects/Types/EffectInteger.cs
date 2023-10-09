using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Effects.Types
{
    public class EffectInteger : EffectBase
    {
        public override byte SerializationId =>
            6;

        public short Value { get; set; }

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectInteger((short)Id, Value);

        public override object[] Values =>
            new object[] { Value };

        public EffectInteger() { }

        public EffectInteger(EffectBase effect, short value) : base(effect) =>
            Value = value;

        public EffectInteger(EffectInteger effect) : base(effect) =>
            Value = effect.Value;

        public override EffectBase GenerateEffect(EffectGenerationType type, EffectGenerationContext context) =>
            new EffectInteger(this);

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);

            writer.WriteShort(Value);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);

            Value = reader.ReadShort();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        public override bool Equals(object? obj) =>
            obj is EffectInteger && base.Equals(obj);

        public static bool operator ==(EffectInteger left, EffectInteger right) =>
            left.Equals(right);

        public static bool operator !=(EffectInteger left, EffectInteger right) =>
            !left.Equals(right);
    }
}
