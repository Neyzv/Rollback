using Rollback.Common.Commands.Types;
using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands
{
    public sealed class StatsCommand : CommandContainer
    {
        public override string[] Aliases =>
            new[] { "stats" };

        public override string Description =>
            "Command to manage stats points of your character";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;
    }

    public sealed class StatsChangeCommand : InGameSubCommand
    {
        public override string[] Aliases =>
            new[] { "change", "c" };

        public override string Description =>
            "Add the amount to the stats points of your character";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public override Type ParentCommand =>
            typeof(StatsCommand);

        public StatsChangeCommand()
        {
            AddParameter("Amount to add.");
            AddParameter("Name of the targeted character.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            var amount = GetParameterValue<short>(0);

            if (amount is not 0)
            {
                Character? targetedCharacter;

                if (GetParameterValue<string?>(1) is not null)
                    targetedCharacter = GetParameterValue<Character>(1);
                else
                    targetedCharacter = sender;

                if (targetedCharacter is not null)
                {
                    targetedCharacter.AddStatsPoints(amount);

                    if (amount > 0)
                        sender.Reply($"{targetedCharacter.Name} have earned {amount} stats points !");
                    else
                        sender.Reply($"{targetedCharacter.Name} have losted {amount} stats points !");
                }
                else
                    sender.ReplyError($"Can not find the target...");
            }
            else
                sender.ReplyError("Incorrect amount value...");
        }
    }

    public sealed class StatsResetCommand : InGameSubCommand
    {
        public override Type ParentCommand =>
            typeof(StatsCommand);

        public override string[] Aliases =>
            new[] { "reset", "r" };

        public override string Description =>
            "Reset the stats of your character.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public StatsResetCommand() =>
            AddParameter("Name of the targeted character.", optional: true);

        protected override void Execute(Character sender)
        {
            Character? targetedCharacter;

            if (GetParameterValue<string?>(0) is not null)
                targetedCharacter = GetParameterValue<Character>(0);
            else
                targetedCharacter = sender;

            if (targetedCharacter is not null)
                targetedCharacter.ResetStats();
            else
                sender.ReplyError($"Can not find the target...");
        }
    }
}
