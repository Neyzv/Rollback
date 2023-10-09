using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Effects.Types
{
    public sealed class EffectDate : EffectBase
    {
        private short _year;
        private short _month;
        private short _day;
        private short _hour;
        private short _minute;

        public override byte SerializationId =>
            3;

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectDate((short)Id, _year, _month, _day, _hour, _minute);

        public override object[] Values =>
            new[] { _year.ToString(), _month.ToString("00") + _day.ToString("00"), _hour.ToString("00") + _minute.ToString("00") };

        public DateTime Date =>
            new(_year, _month, _day, _hour, _minute, 0);

        public EffectDate() { }

        public EffectDate(EffectId id, DateTime date)
        {
            Id = id;
            SetDate(date);
        }

        public override EffectBase GenerateEffect(EffectGenerationType type, EffectGenerationContext context) =>
            new EffectDate(Id, Date);

        public void SetDate(DateTime date)
        {
            _year = (short)date.Year;
            _month = (short)date.Month;
            _day = (short)date.Day;
            _hour = (short)date.Hour;
            _minute = (short)date.Minute;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);
            writer.WriteShort(_year);
            writer.WriteShort(_month);
            writer.WriteShort(_day);
            writer.WriteShort(_hour);
            writer.WriteShort(_minute);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);
            _year = reader.ReadShort();
            _month = reader.ReadShort();
            _day = reader.ReadShort();
            _hour = reader.ReadShort();
            _minute = reader.ReadShort();
        }

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), _year, _month, _day, _hour, _minute);

        public override bool Equals(object? obj) =>
            obj is EffectDate && base.Equals(obj);

        public static bool operator ==(EffectDate left, EffectDate right) =>
            left.Equals(right);

        public static bool operator !=(EffectDate left, EffectDate right) =>
            !left.Equals(right);
    }
}
