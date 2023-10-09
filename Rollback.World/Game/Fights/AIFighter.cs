using Rollback.World.Game.Fights.AI;
using Rollback.World.Game.Looks;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights
{
    public abstract class AIFighter : FightActor
    {
        private readonly Brain _brain;

        public sealed override int Id { get; }

        public virtual bool CanPlay =>
            true;

        protected AIFighter(int contextualId, ActorLook look, StatsData stats, Dictionary<short, Spell> spells, Cell cell)
            : base(look, stats, spells, cell)
        {
            Id = -contextualId;
            _brain = new(this);

            QuitFight += OnAIFighterQuitFight;
        }

        private void OnAIFighterQuitFight() =>
            Stats.Health.Actual = Stats.Health.ActualMax;

        public override bool StartTurn()
        {
            var result = base.StartTurn();
            if (CanPlay && result)
                _brain.Play();

            return result;
        }

        public Spell[] GetSpells(Predicate<Spell>? p = default) =>
            (p is null ? _spells.Values : _spells.Values.Where(x => p(x))).ToArray();
    }
}
