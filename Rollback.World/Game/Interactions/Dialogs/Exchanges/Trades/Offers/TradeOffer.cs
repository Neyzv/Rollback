namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Offers
{
    public sealed class TradeOffer
    {
        public Dictionary<short, int> GivedItemsInformations { get; }

        public int GivedKamas { get; }

        public Dictionary<short, int> NeededItemsInformations { get; }

        public int NeededKamas { get; }

        public TradeOffer(Dictionary<short, int> givedItemsInformations, int givedKamas,
            Dictionary<short, int> neededItemsInformations, int neededKamas)
        {
            GivedItemsInformations = givedItemsInformations;
            GivedKamas = givedKamas;
            NeededItemsInformations = neededItemsInformations;
            NeededKamas = neededKamas;
        }
    }
}
