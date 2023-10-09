using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Spells;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Fights
{
    public sealed class Damage
    {
        public short Amount { get; set; }

        public EffectSchool School { get; }

        public Spell? Spell { get; }

        public EffectInteger? Effect { get; }

        public Zone? Zone { get; }

        public bool ApplyBoost { get; set; }

        public bool ApplyResistance { get; set; }

        public Damage(short amount, EffectSchool school = EffectSchool.Uknown, Zone? zone = default, bool applyBoost = true, bool applyResistance = true)
        {
            Amount = amount;
            School = school;
            Zone = zone;
            ApplyBoost = applyBoost;
            ApplyResistance = applyResistance;
        }

        public Damage(Spell spell, EffectInteger effect, Zone? zone = default, bool applyBoost = true, bool applyResistance = true)
            : this(effect.Value, EffectManager.GetEffectSchool(effect.Id), zone, applyBoost, applyResistance) =>
            (Spell, Effect) = (spell, effect);
    }
}
