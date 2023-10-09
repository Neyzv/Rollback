using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Effects.Types
{
    public sealed class EffectDuration : EffectBase
    {
        public override byte SerializationId =>
            5;

        public short Days { get; set; }

        public short Hours { get; set; }

        public short Minutes { get; set; }

        public override object[] Values =>
            new object[] { Days, Hours, Minutes };

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectDuration((short)Id, Days, Hours, Minutes);

        public EffectDuration() { }

        public EffectDuration(EffectId id, TimeSpan timeSpan)
        {
            Id = id;
            SetDuration(timeSpan);
        }

        public void SetDuration(TimeSpan duration)
        {
            Days = (short)duration.Days;
            Hours = (short)duration.Hours;
            Minutes = (short)duration.Minutes;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);

            writer.WriteShort(Days);
            writer.WriteShort(Hours);
            writer.WriteShort(Minutes);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);

            Days = reader.ReadShort();
            Hours = reader.ReadShort();
            Minutes = reader.ReadShort();
        }

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), Days, Hours, Minutes);

        public override bool Equals(object? obj) =>
            obj is EffectString && base.Equals(obj);

        public static bool operator ==(EffectDuration left, EffectDuration right) =>
            left.Equals(right);

        public static bool operator !=(EffectDuration left, EffectDuration right) =>
            !left.Equals(right);
    }
}
