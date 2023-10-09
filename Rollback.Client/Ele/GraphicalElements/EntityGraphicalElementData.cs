using Rollback.Common.IO.Binary;

namespace Rollback.Client.Ele.GraphicalElements
{
    public sealed class EntityGraphicalElementData : GraphicalElementData
    {
        public override GraphicalElementTypes Type =>
            GraphicalElementTypes.Entity;

        #region Properties
        public string EntityLook { get; set; }

        public bool HorizontalSymmetry { get; set; }
        #endregion

        public EntityGraphicalElementData(int id) : base(id) =>
            EntityLook = string.Empty;

        protected override void InternalSerialize(BigEndianWriter writer)
        {
            writer.WriteInt(EntityLook.Length);
            writer.WriteUTFBytes(EntityLook);

            writer.WriteBoolean(HorizontalSymmetry);
        }

        protected override void Deserialize(BigEndianReader reader)
        {
            EntityLook = reader.ReadUTFBytes(reader.ReadInt());
            HorizontalSymmetry = reader.ReadBoolean();
        }
    }
}
