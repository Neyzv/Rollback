using Rollback.Protocol.Types;
using Rollback.World.Database.Items;

namespace Rollback.World.Game.Accounts
{
    public sealed class Gift
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<KeyValuePair<ItemRecord, int>> Items { get; }

        public StartupActionAddObject StartupActionAddObject =>
            new(Id, Title, Description, string.Empty, string.Empty, Items.Select(x => x.Key.ObjectItemMinimalInformation).ToArray());

        public Gift(int id, string title, string description, List<KeyValuePair<ItemRecord, int>> items)
        {
            Id = id;
            Title = title;
            Description = description;
            Items = items;
        }
    }
}
