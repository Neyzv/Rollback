using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;

namespace Rollback.World.Commands.Commands.Beta
{
    public sealed class DutyFreeCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "shop", "dutyfree" };

        public override string Description =>
            "This command teleport you to the shop zone.";

        public override byte Role =>
            (byte)GameHierarchyEnum.PLAYER;

        protected override void Execute(Character sender) =>
            sender.Teleport(WorldManager.Instance.GetMapById(16778242)!, 328, DirectionsEnum.DIRECTION_SOUTH_WEST);
    }

    public sealed class PvPCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "pvp" };

        public override string Description =>
            "This command teleport you to the pvp map.";

        public override byte Role =>
            (byte)GameHierarchyEnum.PLAYER;

        protected override void Execute(Character sender) =>
            sender.Teleport(WorldManager.Instance.GetMapById(131584)!, 272, DirectionsEnum.DIRECTION_SOUTH_WEST);
    }
}
