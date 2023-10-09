using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Interactives;
using Rollback.World.Game.Interactives.Skills;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Interactives;
using Rollback.World.Handlers.Npcs;

namespace Rollback.World.Game.Interactions.Dialogs.Interactives
{
    public class ZaapDialog : Dialog
    {
        protected readonly Dictionary<int, Map> _destinations;

        public override DialogType DialogType =>
            DialogType.Teleporter;

        public ZaapDialog(Character character, Dictionary<int, Map> destinations) : base(character) =>
            _destinations = destinations;

        protected override void InternalOpen() =>
            InteractiveHandler.SendZaapListMessage(Character.Client, _destinations, GetCost);

        protected override void InternalClose() =>
            NpcHandler.SendLeaveDialogMessage(Character.Client);

        public virtual short GetCost(Map map) =>
            WorldManager.GetDistanceCost(Character.MapInstance.Map.Record.X, Character.MapInstance.Map.Record.Y,
                    map.Record.X, map.Record.Y);

        protected virtual InteractiveObject? GetObjectToTeleport(MapInstance instance) =>
            instance.ContainsZaap ? instance.GetInteractive(x => x.Skills.Values.Any(x => x is ZaapSkill)) : default;

        public void Teleport(int mapId)
        {
            if (_destinations.ContainsKey(mapId))
            {
                var cost = GetCost(_destinations[mapId]);

                if (cost <= Character.Kamas)
                {
                    Close();

                    var instance = _destinations[mapId].GetBestInstance();

                    if (GetObjectToTeleport(instance) is { } interactive)
                        Character.TeleportNear(interactive);
                    else
                        Character.Teleport(instance, instance.GetFirstFreeCellNearMiddle(true).Id);

                    Character.ChangeKamas(-cost);

                }
                else
                    //Vous n\'avez pas assez de kamas pour effectuer cette action.
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 82);
            }
            else
                Character.ReplyError($"Can not find map {mapId} in the destinations...");
        }
    }
}
