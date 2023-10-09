namespace Rollback.World.Game.Items.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ItemTypeAttribute : Attribute
    {
        public short Id { get; set; }

        public ItemTypeAttribute(short id) =>
            Id = id;
    }
}
