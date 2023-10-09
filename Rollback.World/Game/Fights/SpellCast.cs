using Rollback.Protocol.Enums;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Handlers.Spells;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Fights
{
    public sealed class SpellCast
    {
        private readonly SpellEffectHandler[] _handlers;

        public FightActor Caster { get; }

        public Spell Spell { get; }

        public Cell TargetedCell { get; }

        public FightSpellCastCriticalEnum Critical { get; }

        public bool ForceSilentCast { get; set; }

        private Dictionary<int, FightActor>? _affectedActors;
        public Dictionary<int, FightActor> AffectedActors
        {
            get
            {
                if (_affectedActors is null)
                {
                    _affectedActors = new();

                    foreach (var handler in _handlers)
                    {
                        foreach (var target in handler.Target)
                        {
                            if (!_affectedActors.ContainsKey(target.Id))
                                _affectedActors[target.Id] = target;
                        }
                    }
                }

                return _affectedActors;
            }
        }

        public SpellCast(FightActor caster, Spell spell, Cell targetedCell, FightSpellCastCriticalEnum critical)
        {
            Caster = caster;
            Spell = spell;
            TargetedCell = targetedCell;
            Critical = critical;
            _handlers = Critical is FightSpellCastCriticalEnum.CRITICAL_FAIL ? Array.Empty<SpellEffectHandler>() : EffectManager.Instance.GenerateSpellEffectHandler(this);
        }

        public bool IsSilentCast(CharacterFighter target) =>
            ForceSilentCast || _handlers.Any(x => !x.CanSeeCast(target));

        public void ApplyHandlers()
        {
            foreach (var handler in _handlers)
                handler.Apply();
        }
    }
}
