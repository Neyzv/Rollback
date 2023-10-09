using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Effects;
using Rollback.World.Game.Effects.Types;

namespace Rollback.World.Game.Effects
{
    public class EffectBase
    {
        public virtual byte SerializationId =>
            1;

        public EffectId Id { get; set; }

        public uint Random { get; set; }

        public short Duration { get; set; }

        public SpellTargetType TargetType { get; set; }

        public SpellShape Shape { get; set; }

        public byte ZoneSize { get; set; }

        private EffectRecord? _record;
        public EffectRecord? Record =>
            _record ??= EffectManager.Instance.GetEffectRecordById((int)Id);

        public virtual ObjectEffect ObjectEffect =>
            new((short)Id);

        public virtual object[] Values =>
            Array.Empty<object>();

        public EffectBase() { }

        public EffectBase(EffectBase effect)
        {
            Id = effect.Id;
            Random = effect.Random;
            Duration = effect.Duration;
            TargetType = effect.TargetType;
            Shape = effect.Shape;
            ZoneSize = effect.ZoneSize;
        }

        public virtual EffectBase GenerateEffect(EffectGenerationType type, EffectGenerationContext context) =>
            new(this);

        protected virtual void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt((int)Id);
            writer.WriteUInt(Random);
            writer.WriteShort(Duration);
            writer.WriteUShort((ushort)TargetType);
            writer.WriteByte((byte)Shape);
            writer.WriteByte(ZoneSize);
        }

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteByte(SerializationId);
            InternalSerialize(writer);
        }

        protected virtual void InternalDeserialize(BigEndianReader reader)
        {
            Id = (EffectId)reader.ReadInt();
            Random = reader.ReadUInt();
            Duration = reader.ReadShort();
            TargetType = (SpellTargetType)reader.ReadUShort();
            Shape = (SpellShape)reader.ReadByte();
            ZoneSize = reader.ReadByte();
        }

        public static EffectBase Deserialize(BigEndianReader reader)
        {
            var serializationId = reader.ReadByte();

            EffectBase effect = serializationId switch
            {
                1 => new(),
                //2 => new EffectCreature(),
                3 => new EffectDate(),
                4 => new EffectDice(),
                5 => new EffectDuration(),
                6 => new EffectInteger(),
                /*7 => new EffectLadder(),
                8 => new EffectMinMax(),*/
                9 => new EffectMount(),
                10 => new EffectString(),
                _ => throw new Exception($"Uknown identifier {serializationId} while deserializing effects...")
            };

            effect.InternalDeserialize(reader);

            return effect;
        }

        public EffectBase Clone() =>
            (EffectBase)MemberwiseClone();

        public override int GetHashCode() =>
            Id.GetHashCode() + 3287;

        public override bool Equals(object? obj) =>
            obj is not null && obj is EffectBase && (ReferenceEquals(this, obj) || GetHashCode() == obj.GetHashCode());

        public static bool operator ==(EffectBase left, EffectBase right) =>
            left.Equals(right);

        public static bool operator !=(EffectBase left, EffectBase right) =>
            !left.Equals(right);
    }
}
