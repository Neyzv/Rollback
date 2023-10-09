using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Items;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Offers;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Items;
using Rollback.World.Game.Items.Types;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types
{
    public sealed class PetResurrectionTrade : NpcTrade
    {
        private const short ResurectionPowderItemId = 8012;

        private static Lazy<List<TradeOffer>> _petsTradeOffers;
        public static List<TradeOffer> PetsTradeOffers =>
            _petsTradeOffers.Value;

        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.NPC_RESURECT_PET;

        static PetResurrectionTrade() =>
            _petsTradeOffers = new Lazy<List<TradeOffer>>(() => InitTradeOffers(), LazyThreadSafetyMode.ExecutionAndPublication);

        public PetResurrectionTrade(KamasDisabledTrader sender, NpcTrader receiver)
            : base(sender, receiver, PetsTradeOffers) { }

        private static List<TradeOffer> InitTradeOffers()
        {
            var tradeOffers = new List<TradeOffer>();

            foreach (var petInformations in ItemManager.Instance.GetPetsRecords())
            {
                tradeOffers.Add(new TradeOffer(
                    new Dictionary<short, int>()
                    {
                        [petInformations.PetId] = 1,
                    },
                    0,
                    new Dictionary<short, int>()
                    {
                        [petInformations.GhostId] = 1,
                        [ResurectionPowderItemId] = 1,
                    },
                    0
                ));
            }

            return tradeOffers;
        }

        protected override PlayerItem? CreateTraderItem(ItemRecord template, int quantity, TradeOffer tradeOffer)
        {
            var result = default(PlayerItem?);

            if (Sender.Items.FirstOrDefault(x => x.TypeId is ItemType.FantomeDeFamilier) is { } ghostItem)
            {
                result = ItemManager.Instance.CreatePlayerItem(Sender.Character.Inventory,
                        template,
                        quantity,
                        EffectGenerationType.Normal,
                        ghostItem.Effects.Select(x => x.Clone())
                    );

                if (result.Effects.OfType<EffectDice>().FirstOrDefault(x => x.Id is EffectId.EffectLifePoints) is { } lifePointsEffect)
                    lifePointsEffect.Value = 1;
            }

            return result;
        }
    }
}
