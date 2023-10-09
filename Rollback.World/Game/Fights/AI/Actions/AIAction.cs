namespace Rollback.World.Game.Fights.AI.Actions
{
    public abstract class AIAction
    {
        protected readonly AIFighter _fighter;

        public AIAction(AIFighter fighter) =>
            _fighter = fighter;

        public abstract void Apply();
    }
}
