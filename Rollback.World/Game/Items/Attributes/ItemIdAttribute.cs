namespace Rollback.World.Game.Items.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ItemIdAttribute : Attribute
    {
        public short Id { get; set; }

        public ItemIdAttribute(short id) =>
            Id = id;
    }
}
