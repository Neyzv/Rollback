using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Items;
using Rollback.World.Game.RolePlayActors.TaxCollectors;

namespace Rollback.World.Game.Fights.Results.Types
{
    public sealed class TaxCollectorProspectingResult : FightResult<TaxCollectorNpc>, IExperienceFightResult
    {
        private int _experience;
        private int _dropPods;

        public override bool CanDrop =>
            true;

        public override short Wisdom =>
            Looter.TaxCollector.Guild.TaxCollectorWisdom;

        public override short Prospecting =>
            Looter.TaxCollector.Guild.TaxCollectorProspecting;

        public TaxCollectorProspectingResult(TaxCollectorNpc taxCollector) : base(taxCollector) { }

        public void AddEarnedExperience(int amount)
        {
            if (amount > 0)
            {
                var xp = (int)Math.Floor(amount * 0.1);
                _experience += xp > TaxCollectorManager.MaxGatheredXPPerFight ? TaxCollectorManager.MaxGatheredXPPerFight : xp;
            }
        }

        public override void AddEarnedItem(short itemId, short quantity)
        {
            if (Looter.TaxCollector.Bag.Pods + _dropPods < Looter.TaxCollector.Bag.MaxPods &&
                ItemManager.Instance.GetTemplateRecordById(itemId) is { } template)
            {
                base.AddEarnedItem(itemId, quantity);
                _dropPods += template.Weight * quantity;
            }
        }

        public override void Apply()
        {
            Looter.TaxCollector.ChangeGatheredExperience(_experience);
            Looter.TaxCollector.ChangeGatheredKamas(Kamas);

            foreach (var droppedItem in _itemsToLoot)
                if (ItemManager.Instance.GetTemplateRecordById(droppedItem.Key) is { } template)
                    Looter.TaxCollector.Bag.AddItem(ItemManager.Instance.CreateTaxCollectorItem(Looter.TaxCollector.Bag, template, droppedItem.Value, EffectGenerationType.Normal));

            _experience = default;
            _itemsToLoot.Clear();
        }

        public override FightResultFighterListEntry GetResult(FightOutcomeEnum outcome) =>
            new FightResultTaxCollectorListEntry((sbyte)FightOutcomeEnum.RESULT_TAX, Loot, Looter.Id, true, Looter.TaxCollector.Guild.Level,
                Looter.TaxCollector.Guild.Name, _experience);
    }
}
