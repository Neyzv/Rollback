using Rollback.World.Game.Fights.History.Actions;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights.History
{
    public sealed class FightHistory
    {
        private readonly Dictionary<short, Dictionary<short, SpellCastAction>> _spellCasts;
        private readonly List<Cell> _movements;

        public FightHistory()
        {
            _spellCasts = new();
            _movements = new();
        }

        public void RegisterCast(SpellCast cast, short roundNumber)
        {
            if (!_spellCasts.TryGetValue(roundNumber, out var casts))
            {
                casts = new();
                _spellCasts[roundNumber] = casts;
            }

            if (!casts.TryGetValue(cast.Spell.Id, out var castAction))
            {
                castAction = new(cast.Spell);
                casts[cast.Spell.Id] = castAction;
            }

            castAction.IncrementCastCount();

            foreach (var target in cast.AffectedActors.Values)
                castAction.HitTarget(target.Id);
        }

        public void RegisterMovement(Cell cell) =>
            _movements.Add(cell);

        public short? GetLastRoundCast(short spellId)
        {
            foreach (var casts in _spellCasts.OrderByDescending(x => x.Key))
            {
                if (casts.Value.ContainsKey(spellId))
                    return casts.Key;
            }

            return null;
        }

        public short GetAmountOfCast(short roundNumber, short spellId) =>
            _spellCasts.ContainsKey(roundNumber) && _spellCasts[roundNumber].ContainsKey(spellId) ?
                _spellCasts[roundNumber][spellId].CastCount : (short)0;

        public short GetAmountOfCastForTarget(short roundNumber, short spellId, int actorId) =>
            _spellCasts.ContainsKey(roundNumber) && _spellCasts[roundNumber].ContainsKey(spellId) ?
                _spellCasts[roundNumber][spellId].GetAmountOfCastFor(actorId) : (short)0;
    }
}
