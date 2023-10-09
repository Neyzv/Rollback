using Rollback.Common.Commands;
using Rollback.Common.Commands.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Commands
{
    public abstract class InGameSubCommand : SubCommand
    {
        public override sealed void Execute(ICommandUser sender)
        {
            if (sender is not Character character)
                throw new InvalidCastException();

            Execute(character);
        }

        protected abstract void Execute(Character sender);
    }
}
