using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Effects.Types
{
    public sealed class EffectString : EffectBase
    {
        public override byte SerializationId =>
            10;

        public string Value { get; set; }

        public override object[] Values =>
            new[] { Value };

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectString((short)Id, Value);

        public EffectString() =>
            Value = string.Empty;

        public EffectString(EffectId id, string value) =>
            (Id, Value) = (id, value);

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);

            writer.WriteString(Value);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);

            Value = reader.ReadString();
        }

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), Value);

        public override bool Equals(object? obj) =>
            obj is EffectString && base.Equals(obj);

        public static bool operator ==(EffectString left, EffectString right) =>
            left.Equals(right);

        public static bool operator !=(EffectString left, EffectString right) =>
            !left.Equals(right);
    }
}
