using Rollback.World.Game.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Interactives;

namespace Rollback.World.Game.Interactions.Dialogs.Interactives
{
    public sealed class ZaapiDialog : ZaapDialog
    {
        public ZaapiDialog(Character character, Dictionary<int, Map> destinations) : base(character, destinations) { }

        protected override void InternalOpen() =>
            InteractiveHandler.SendTeleportDestinationsListMessage(Character.Client, CustomEnums.TeleporterType.Zaapi, _destinations, GetCost);

        public override short GetCost(Map map) =>
            20;

        protected override InteractiveObject? GetObjectToTeleport(MapInstance instance) =>
            instance.GetInteractive(x => x.Skills.Values.Any(x => x is ZaapiSkill));
    }
}
