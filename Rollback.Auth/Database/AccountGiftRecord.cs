using Rollback.Common.Logging;
using Rollback.Common.Network.IPC.Types.Gifts;
using Rollback.Common.ORM;

namespace Rollback.Auth.Database
{
    public static class AccountGiftRelator
    {
        public const string GetAccountGiftsByAccountId = "SELECT * FROM accounts_gifts WHERE AccountId = {0}";
        public const string DeleteAccountGiftById = "DELETE FROM accounts_gifts WHERE Id = {0}";
    }

    [Table("accounts_gifts")]
    public sealed record AccountGiftRecord
    {
        public AccountGiftRecord()
        {
            Title = string.Empty;
            Description = string.Empty;
            _itemsCSV = string.Empty;
            Items = Array.Empty<KeyValuePair<short, int>>();
            _unavailableServerIdsCSV = string.Empty;
            UnavailableServerIds = Array.Empty<int>();
        }

        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        private string _itemsCSV;
        public string ItemsCSV
        {
            get => _itemsCSV;
            set
            {
                _itemsCSV = value;

                var giftInfos = value.Split(';').Where(x => x != string.Empty).ToArray();
                Items = new KeyValuePair<short, int>[giftInfos.Length];
                for (int i = 0; i < giftInfos.Length; i++)
                {
                    var itemInfos = giftInfos[i].Split(',');

                    if (itemInfos.Length > 1 && short.TryParse(itemInfos[0], out var itemId) && int.TryParse(itemInfos[1], out var quantity))
                        Items[i] = new(itemId, quantity);
                    else
                        Logger.Instance.LogWarn($"Can not parse item infos of gift {Id}...");
                }
            }
        }

        [Ignore]
        public KeyValuePair<short, int>[] Items { get; set; }

        private string _unavailableServerIdsCSV;
        public string UnavailableServerIdsCSV
        {
            get => _unavailableServerIdsCSV;
            set
            {
                _unavailableServerIdsCSV = value;

                try
                {
                    UnavailableServerIds = value.Split(',').Where(x => x != string.Empty).Select(x => int.Parse(x)).ToArray();
                }
                catch
                {
                    Logger.Instance.LogError(msg: $"Can not parse unavailable server ids of gift {Id}...");
                }
            }
        }

        [Ignore]
        public int[] UnavailableServerIds { get; set; }

        [Ignore]
        public GiftInformations GiftInformations =>
            new(Id, Title, Description, Items);
    }
}
