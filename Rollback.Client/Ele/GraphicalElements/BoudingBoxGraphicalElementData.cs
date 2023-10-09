namespace Rollback.Client.Ele.GraphicalElements
{
    public sealed class BoudingBoxGraphicalElementData : NormalGraphicalElementData
    {
        public override GraphicalElementTypes Type =>
            GraphicalElementTypes.BoudingBox;

        public BoudingBoxGraphicalElementData(int id) : base(id) { }
    }
}
