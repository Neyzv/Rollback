using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands.Beta
{
    public sealed class AddLevelCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "addlvl" };

        public override string Description =>
            "This command set your level to the desire amount.";

        public override byte Role =>
            (byte)GameHierarchyEnum.PLAYER;

        public AddLevelCommand() =>
            AddParameter("Amount of level to add to the character.");

        protected override void Execute(Character sender) =>
            sender.LevelUp(GetParameterValue<byte>(0));
    }
}
