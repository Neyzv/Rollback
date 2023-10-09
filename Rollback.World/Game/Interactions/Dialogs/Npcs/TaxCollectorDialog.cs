using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Npcs
{
    public sealed class TaxCollectorDialog : Dialog<TaxCollectorNpc>
    {
        public TaxCollectorDialog(Character character, TaxCollectorNpc dialoger)
            : base(character, dialoger) { }

        public override DialogType DialogType =>
            DialogType.Dialog;

        protected override void InternalOpen()
        {
            NpcHandler.SendNpcDialogCreationMessage(Character.Client, Dialoger);
            NpcHandler.SendTaxCollectorDialogQuestionExtendedMessage(Character.Client, Dialoger);
        }

        protected override void InternalClose() =>
            NpcHandler.SendLeaveDialogMessage(Character.Client);
    }
}
