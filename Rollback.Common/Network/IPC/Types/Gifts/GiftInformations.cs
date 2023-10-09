#nullable disable

using Rollback.Common.IO.Binary;

namespace Rollback.Common.Network.IPC.Types.Gifts
{
    public sealed record GiftInformations
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public KeyValuePair<short, int>[] Items { get; set; }

        public GiftInformations() { }

        public GiftInformations(int id, string title, string description, KeyValuePair<short, int>[] items)
        {
            Id = id;
            Title = title;
            Description = description;
            Items = items;
        }

        public void Serialize(BigEndianWriter writer)
        {
            writer.WriteInt(Id);
            writer.WriteString(Title);
            writer.WriteString(Description);

            writer.WriteUShort((ushort)Items.Length);
            for (var i = 0; i < Items.Length; i++)
            {
                writer.WriteShort(Items[i].Key);
                writer.WriteInt(Items[i].Value);
            }
        }

        public void Deserialize(BigEndianReader reader)
        {
            Id = reader.ReadInt();
            Title = reader.ReadString();
            Description = reader.ReadString();

            var itemsCount = reader.ReadUShort();
            Items = new KeyValuePair<short, int>[itemsCount];
            for (var i = 0; i < itemsCount; i++)
                Items[i] = new(reader.ReadShort(), reader.ReadInt());
        }
    }
}
