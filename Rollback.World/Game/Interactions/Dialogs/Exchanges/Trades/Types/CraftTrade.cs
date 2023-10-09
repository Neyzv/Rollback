using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Traders;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.Items;
using Rollback.World.Game.Jobs;
using Rollback.World.Handlers.Exchanges;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Trades.Types
{
    public sealed class CraftTrade : Trade<Trader>
    {
        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.CRAFT;

        public Skill Skill { get; }

        public CraftTrade(KamasDisabledTrader sender, Trader receiver, Skill skill)
            : base(sender, receiver) =>
            Skill = skill;

        protected override void InternalOpen() =>
            ExchangeHandler.SendExchangeStartOkCraftWithInformationMessage(Sender.Character.Client, Skill);

        protected override void ProcessTrade()
        {
            if ((!Sender.Character.Jobs.ContainsKey(Skill.ParentJobId!.Value) || (Skill.MinJobLevel <= Sender.Character.Jobs[Skill.ParentJobId.Value].Level &&
                Sender.Items.Count <= JobManager.GetCraftMaxSlotsCount(Sender.Character.Jobs[Skill.ParentJobId.Value].Level))) &&
                JobManager.Instance.GetRecipeResult(Sender.Items.Select(x => (x.Id, x.Quantity)).ToArray()) is { } result &&
                ItemManager.Instance.GetTemplateRecordById(result) is not null)
            {
                foreach (var ingredient in Sender.Items)
                    Sender.Character.Inventory.RemoveItem(ingredient.UID, ingredient.Quantity);

                ClearItems(Sender);

                var craftResult = !Sender.Character.Jobs.ContainsKey(Skill.ParentJobId.Value) ||
                    Random.Shared.Next(101) <= JobManager.GetCraftSuccessPercentage(Sender.Character.Jobs[Skill.ParentJobId.Value].Level)
                    ? ExchangeCraftResult.Success
                    : ExchangeCraftResult.Failed;

                if (craftResult is ExchangeCraftResult.Success)
                    ExchangeHandler.SendExchangeCraftResultWithObjectDescMessage(Sender.Character.Client, craftResult,
                        Sender.Character.Inventory.AddItem(result, send: false)!);
                else
                    ExchangeHandler.SendExchangeCraftResultWithObjectIdMessage(Sender.Character.Client, craftResult, result);

                Sender.Character.MapInstance.Send(ExchangeHandler.SendExchangeCraftInformationObjectMessage,
                    new object[] { craftResult, result, Sender.Character });

                if (Sender.Character.Jobs.ContainsKey(Skill.ParentJobId.Value))
                    Sender.Character.Jobs[Skill.ParentJobId.Value].ChangeExperience(JobManager.GetCraftJobXp(Sender.Character.Jobs[Skill.ParentJobId.Value].Level,
                        Sender.Items.Count));
            }
            else
            {
                ClearItems(Sender);
                ExchangeHandler.SendExchangeCraftResultMessage(Sender.Character.Client, ExchangeCraftResult.Failed);
            }
        }

        public void ReplayTrade(int count)
        {
            // TO DO
        }
    }
}
