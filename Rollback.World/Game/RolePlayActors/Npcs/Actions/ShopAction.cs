using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Npcs;
using Rollback.World.Game.Interactions.Dialogs.Npcs;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.RolePlayActors.Npcs.Actions
{
    [Identifier("Shop")]
    public sealed class ShopAction : NpcAction
    {
        private bool? _canSell;
        public bool CanSell =>
            (_canSell ??= GetParameterValue<bool?>(0)) is not null && _canSell!.Value;

        public override NpcActionType NpcActionType =>
            NpcActionType.BuySell;

        public ShopAction(NpcActionRecord record) : base(record) { }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcShopDialog(character, npc, Items, CanSell);
            dialog.Open();
        }
    }
}
