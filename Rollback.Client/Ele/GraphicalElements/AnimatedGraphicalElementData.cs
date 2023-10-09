namespace Rollback.Client.Ele.GraphicalElements
{
    public sealed class AnimatedGraphicalElementData : NormalGraphicalElementData
    {
        public override GraphicalElementTypes Type =>
            GraphicalElementTypes.Animated;

        public AnimatedGraphicalElementData(int id) : base(id) { }
    }
}
