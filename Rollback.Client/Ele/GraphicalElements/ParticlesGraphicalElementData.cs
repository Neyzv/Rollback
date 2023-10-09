using Rollback.Common.IO.Binary;

namespace Rollback.Client.Ele.GraphicalElements
{
    public sealed class ParticlesGraphicalElementData : GraphicalElementData
    {
        public override GraphicalElementTypes Type =>
            GraphicalElementTypes.Particles;

        public short ScriptId { get; set; }

        public ParticlesGraphicalElementData(int id) : base(id) { }

        protected override void InternalSerialize(BigEndianWriter writer) =>
            writer.WriteShort(ScriptId);

        protected override void Deserialize(BigEndianReader reader) =>
            ScriptId = reader.ReadShort();
    }
}
