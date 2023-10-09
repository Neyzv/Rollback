using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Offers;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types
{
    public sealed class ChanilTrade : NpcTrade
    {
        private const byte KamasCostPerMealHours = 10;

        private static Lazy<List<TradeOffer>> _petsTradeOffers;
        public static List<TradeOffer> PetsTradeOffers =>
            _petsTradeOffers.Value;

        static ChanilTrade() =>
            _petsTradeOffers = new Lazy<List<TradeOffer>>(() => InitTradeOffers(), LazyThreadSafetyMode.ExecutionAndPublication);

        public ChanilTrade(PlayerTrader sender, NpcTrader receiver)
            : base(sender, receiver, PetsTradeOffers) { }

        private static List<TradeOffer> InitTradeOffers()
        {
            var result = new List<TradeOffer>();

            foreach (var petInformations in ItemManager.Instance.GetPetsRecords())
            {
                if (petInformations.CertificateId.HasValue)
                {
                    result.Add(new TradeOffer(
                        new Dictionary<short, int>()
                        {
                            [petInformations.PetId] = 1,
                        },
                        0,
                        new Dictionary<short, int>()
                        {
                            [petInformations.CertificateId.Value] = 1
                        },
                        0
                    ));

                    result.Add(new TradeOffer(
                        new Dictionary<short, int>()
                        {
                            [petInformations.CertificateId.Value] = 1,
                        },
                        0,
                        new Dictionary<short, int>()
                        {
                            [petInformations.PetId] = 1
                        },
                        0
                    ));
                }
            }

            return result;
        }

        protected override PlayerItem? CreateTraderItem(ItemRecord template, int quantity, TradeOffer tradeOffer)
        {
            var result = default(PlayerItem?);

            if (Sender.Items.FirstOrDefault(x => x.TypeId is ItemType.CertificatDeFamilier or ItemType.Familier) is { } neededItem)
            {
                var petCost = 0;

                if (neededItem.TypeId is ItemType.CertificatDeFamilier && neededItem.Effects.OfType<EffectDate>()
                    .FirstOrDefault(x => x.Id is EffectId.EffectLastMealDate) is { } neededItemlastMealDateEffect)
                    petCost = 1 + (DateTime.Now - neededItemlastMealDateEffect.Date).Hours * KamasCostPerMealHours;

                if (Sender.Character.Kamas >= petCost)
                {
                    Sender.Character.ChangeKamas(-petCost);

                    result = ItemManager.Instance.CreatePlayerItem(Sender.Character.Inventory,
                            template,
                            quantity,
                            EffectGenerationType.Normal,
                            neededItem.Effects.Select(x => x.Clone())
                        );

                    if (result.Effects.OfType<EffectDate>().FirstOrDefault(x => x.Id is EffectId.EffectLastMealDate) is { } lastMealDateEffect)
                        lastMealDateEffect.SetDate(DateTime.Now);
                }
            }

            return result;
        }
    }
}
