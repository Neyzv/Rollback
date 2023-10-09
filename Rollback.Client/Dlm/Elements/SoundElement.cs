using Rollback.Common.IO.Binary;

namespace Rollback.Client.Dlm.Elements
{
    public sealed class SoundElement : BasicElement
    {
        #region Properties
        public override ElementTypes Type =>
            ElementTypes.Sound;

        public int SoundId { get; set; }

        public short BaseVolume { get; set; }

        public int FullVolumeDistance { get; set; }

        public int NullVolumeDistance { get; set; }

        public short MinDelayBetweenLoops { get; set; }

        public short MaxDelayBetweenLoops { get; set; }
        #endregion

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(SoundId);

            writer.WriteShort(BaseVolume);

            writer.WriteInt(FullVolumeDistance);
            writer.WriteInt(NullVolumeDistance);

            writer.WriteShort(MinDelayBetweenLoops);
            writer.WriteShort(MaxDelayBetweenLoops);
        }

        protected override void Deserialize(BigEndianReader reader)
        {
            SoundId = reader.ReadInt();

            BaseVolume = reader.ReadShort();

            FullVolumeDistance = reader.ReadInt();
            NullVolumeDistance = reader.ReadInt();

            MinDelayBetweenLoops = reader.ReadShort();
            MaxDelayBetweenLoops = reader.ReadShort();
        }
    }
}
