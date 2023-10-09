using Rollback.Protocol.Types;
using Rollback.World.Game.RolePlayActors.Characters;

namespace Rollback.World.Game.Fights.Results
{
    public abstract class FightAdditionalDatas
    {
        public Character Character { get; }

        public abstract FightResultAdditionalData FightResultAdditionalData { get; }

        protected FightAdditionalDatas(Character character) =>
            Character = character;

        public abstract void Apply();
    }
}
