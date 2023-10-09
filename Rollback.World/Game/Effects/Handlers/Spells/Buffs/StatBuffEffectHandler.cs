using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Logging;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectAddAgility), Identifier(EffectId.EffectAddChance), Identifier(EffectId.EffectAddIntelligence),
        Identifier(EffectId.EffectAddStrength), Identifier(EffectId.EffectAddWisdom), Identifier(EffectId.EffectAddVitality),
        Identifier(EffectId.EffectAddRange), Identifier(EffectId.EffectAddRange136), Identifier(EffectId.EffectAddSummonLimit),
        Identifier(EffectId.EffectAddDamageBonus), Identifier(EffectId.EffectAddDamageBonus121), Identifier(EffectId.EffectAddHealBonus),
        Identifier(EffectId.EffectIncreaseDamage138), Identifier(EffectId.EffectAddDamageBonusPercent), Identifier(EffectId.EffectAddDamageReflection),
        Identifier(EffectId.EffectAddDamageReflection220), Identifier(EffectId.EffectAddDodgeAPProbability), Identifier(EffectId.EffectAddDodgeMPProbability),
        Identifier(EffectId.EffectAddCriticalHit), Identifier(EffectId.EffectAddCriticalMiss), Identifier(EffectId.EffectAddAirResistPercent),
        Identifier(EffectId.EffectAddFireResistPercent), Identifier(EffectId.EffectAddEarthResistPercent), Identifier(EffectId.EffectAddWaterResistPercent),
        Identifier(EffectId.EffectAddNeutralResistPercent), Identifier(EffectId.EffectAddProspecting), Identifier(EffectId.EffectAddTrapBonusPercent),
        Identifier(EffectId.EffectAddAP111), Identifier(EffectId.EffectRegainAP), Identifier(EffectId.EffectAddMP), Identifier(EffectId.EffectAddMP128),
        Identifier(EffectId.EffectAddDamageMultiplicator), Identifier(EffectId.EffectAddPhysicalDamage142), Identifier(EffectId.EffectAddPhysicalDamage137),
        Identifier(EffectId.EffectAddMagicDamageReduction), Identifier(EffectId.EffectAddPhysicalDamageReduction), Identifier(EffectId.EffectAddErosion)]
    public sealed class StatBuffEffectHandler : SpellEffectHandler
    {
        public StatBuffEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            var stat = GetEffectStat(Effect.Id);
            if (stat is not null)
            {
                var effectInteger = GenerateEffect();
                if (effectInteger is not null)
                {
                    if (effectInteger.Duration is not 0 || fighter.Id == Cast.Caster.Id)
                        fighter.AddStatBuff(this, effectInteger.Value, stat.Value,
                            effectInteger.Id is EffectId.EffectRegainAP ? (short)EffectId.EffectAddAP111 :
                            effectInteger.Id is EffectId.EffectAddMP ? (short)EffectId.EffectAddMP128 : null);
                    else if (stat is Stat.ActionPoints)
                        fighter.RegainAP(effectInteger.Value);
                    else if (stat is Stat.MovementPoints)
                        fighter.RegainMP(effectInteger.Value);
                }
            }
            else
                Logger.Instance.LogWarn($"{Effect.Id} hasn't a binded stat...");
        }

        private static Stat? GetEffectStat(EffectId effect) =>
            effect switch
            {
                EffectId.EffectAddAgility => Stat.Agility,
                EffectId.EffectAddChance => Stat.Chance,
                EffectId.EffectAddIntelligence => Stat.Intelligence,
                EffectId.EffectAddStrength => Stat.Strength,
                EffectId.EffectAddWisdom => Stat.Wisdom,
                EffectId.EffectAddVitality => Stat.Vitality,
                EffectId.EffectAddRange or EffectId.EffectAddRange136 => Stat.Range,
                EffectId.EffectAddSummonLimit => Stat.SummonableCreaturesBoost,
                EffectId.EffectAddDamageBonus or EffectId.EffectAddDamageBonus121 => Stat.AllDamagesBonus,
                EffectId.EffectAddHealBonus => Stat.HealBonus,
                EffectId.EffectIncreaseDamage138 or EffectId.EffectAddDamageBonusPercent => Stat.DamagesBonusPercent,
                EffectId.EffectAddDamageReflection or EffectId.EffectAddDamageReflection220 => Stat.Reflect,
                EffectId.EffectAddDodgeAPProbability => Stat.DodgeApLostProbability,
                EffectId.EffectAddDodgeMPProbability => Stat.DodgeMpLostProbability,
                EffectId.EffectAddCriticalHit => Stat.CriticalHit,
                EffectId.EffectAddCriticalMiss => Stat.CriticalMiss,
                EffectId.EffectAddAirResistPercent => Stat.AirElementResistPercent,
                EffectId.EffectAddFireResistPercent => Stat.FireElementResistPercent,
                EffectId.EffectAddEarthResistPercent => Stat.EarthElementResistPercent,
                EffectId.EffectAddWaterResistPercent => Stat.WaterElementResistPercent,
                EffectId.EffectAddNeutralResistPercent => Stat.NeutralElementResistPercent,
                EffectId.EffectAddProspecting => Stat.Prospecting,
                EffectId.EffectAddTrapBonusPercent => Stat.TrapBonusPercent,
                EffectId.EffectAddAP111 or EffectId.EffectRegainAP => Stat.ActionPoints,
                EffectId.EffectAddMP or EffectId.EffectAddMP128 => Stat.MovementPoints,
                EffectId.EffectAddDamageMultiplicator => Stat.PermanentDamagePercent,
                EffectId.EffectAddPhysicalDamage137 or EffectId.EffectAddPhysicalDamage142 => Stat.PhysicalDamage,
                EffectId.EffectAddMagicDamageReduction => Stat.MagicalReduction,
                EffectId.EffectAddPhysicalDamageReduction => Stat.PhysicalReduction,
                EffectId.EffectAddErosion => Stat.Erosion,
                _ => default
            };
    }
}
