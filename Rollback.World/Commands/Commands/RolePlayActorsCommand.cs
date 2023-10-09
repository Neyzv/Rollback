using Rollback.Common.Commands.Types;
using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands
{
    public sealed class RolePlayActorsCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "actors" };

        public override string Description =>
            "Commands to manage actors.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class RolePlayActorInfosCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(RolePlayActorsCommand);

        public override string[] Aliases =>
            new[] { "infos", "i" };

        public override string Description => throw new NotImplementedException();

        public override byte Role => throw new NotImplementedException();

        protected override void Execute(Character sender)
        {
            throw new NotImplementedException();
        }
    }
}
