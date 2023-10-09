using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.Npcs;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Npcs
{
    public sealed class NpcForgetSpellDialog : Dialog<Npc>
    {
        public override DialogType DialogType =>
            DialogType.SpellForget;

        public NpcForgetSpellDialog(Character character, Npc dialoger) : base(character, dialoger) { }

        protected override void InternalOpen() =>
            NpcHandler.SendSpellForgetUIMessage(Character.Client, true);

        protected override void InternalClose() =>
            NpcHandler.SendSpellForgetUIMessage(Character.Client, false);
    }
}
