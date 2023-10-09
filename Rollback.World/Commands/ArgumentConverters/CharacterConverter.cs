using Rollback.Common.Commands;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Network;

namespace Rollback.World.Commands.ArgumentConverters
{
    public class CharacterConverter : IArgumentConverter<Character>
    {
        public Character? Convert(object value)
        {
            var p = default(Predicate<WorldClient>?);

            if (value is string name && CharacterManager.IsNameCorrect(name))
                p = x => x.Account!.Character?.Name == name;
            else if (int.TryParse(value.ToString(), out var id))
                p = x => x.Account!.Character?.Id == id;

            return p is not null ? WorldServer.Instance.GetClient(x => p(x))?.Account?.Character : default;
        }
    }
}
