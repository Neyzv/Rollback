using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands
{
    public sealed class KamasCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "kamas" };

        public override string Description =>
            "Command to manage character's kamas";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public KamasCommand()
        {
            AddParameter("Amount to change.");
            AddParameter("Character to target.", optional: true);
        }

        protected override void Execute(Character sender)
        {
            Character? targetedCharacter;

            if (GetParameterValue<string?>(1) is not null)
                targetedCharacter = GetParameterValue<Character>(1);
            else
                targetedCharacter = sender;

            if (targetedCharacter is not null)
            {
                var amount = GetParameterValue<int>(0);
                targetedCharacter.ChangeKamas(amount);
            }
            else
                sender.ReplyError($"Can not find the target...");
        }
    }
}
