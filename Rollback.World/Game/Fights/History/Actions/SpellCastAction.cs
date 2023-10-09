using Rollback.World.Game.Spells;

namespace Rollback.World.Game.Fights.History.Actions
{
    public sealed class SpellCastAction
    {
        private readonly Dictionary<int, short> _affectedActors;

        public Spell Spell { get; }

        public short CastCount { get; private set; }

        public SpellCastAction(Spell spell)
        {
            Spell = spell;
            _affectedActors = new();
        }

        public short GetAmountOfCastFor(int actorId)
        {
            _affectedActors.TryGetValue(actorId, out var amount);

            return amount;
        }

        public void IncrementCastCount() =>
            CastCount++;

        public void HitTarget(int targetId)
        {
            if (_affectedActors.ContainsKey(targetId))
                _affectedActors[targetId] = _affectedActors[targetId]++;
            else
                _affectedActors[targetId] = 1;
        }
    }
}
