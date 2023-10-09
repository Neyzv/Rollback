using Rollback.Protocol.Enums;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands.Commands
{
    public sealed class LevelUpCommand : InGameCommand
    {
        public override string[] Aliases =>
            new[] { "levelup", "addlevel", "lvl" };

        public override string Description =>
            "Command to add some level to the targeted character.";

        public override byte Role =>
            (byte)GameHierarchyEnum.ADMIN;

        public LevelUpCommand()
        {
            AddParameter("Amount of level to add to the character.");
            AddParameter("Name of the targeted character.", optional: true);
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
                var amount = GetParameterValue<byte>(0);
                targetedCharacter.LevelUp(amount);
                sender.Reply($"Added {amount} levels to {targetedCharacter.Name} !");
            }
            else
                sender.ReplyError($"Can not find the target...");
        }
    }
}
