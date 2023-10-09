using Rollback.Common.IO.Binary;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;

namespace Rollback.World.Game.Effects.Types
{
    public sealed class EffectMount : EffectBase
    {
        public override byte SerializationId =>
            9;

        public int MountId { get; set; }

        public double ExpirationDate { get; set; }

        public short ModelId { get; set; }

        public override object[] Values =>
            new object[] { MountId, ExpirationDate, ModelId };

        public override ObjectEffect ObjectEffect =>
            new ObjectEffectMount((short)Id, MountId, ExpirationDate, ModelId);

        public EffectMount() { }

        public EffectMount(int mountId, DateTimeOffset expirationDate, short modelId)
        {
            Id = EffectId.EffectViewMountCharacteristics;
            MountId = mountId;
            ExpirationDate = expirationDate.ToUnixTimeMilliseconds();
            ModelId = modelId;
        }

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            base.InternalSerialize(writer);

            writer.WriteInt(MountId);
            writer.WriteDouble(ExpirationDate);
            writer.WriteShort(ModelId);
        }

        protected override void InternalDeserialize(BigEndianReader reader)
        {
            base.InternalDeserialize(reader);

            MountId = reader.ReadInt();
            ExpirationDate = reader.ReadDouble();
            ModelId = reader.ReadShort();
        }

        public override int GetHashCode() =>
            HashCode.Combine(base.GetHashCode(), MountId, ExpirationDate, ModelId);

        public override bool Equals(object? obj) =>
            obj is EffectString && base.Equals(obj);

        public static bool operator ==(EffectMount left, EffectMount right) =>
            left.Equals(right);

        public static bool operator !=(EffectMount left, EffectMount right) =>
            !left.Equals(right);
    }
}
