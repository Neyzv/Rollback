using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Logging;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectSubAgility), Identifier(EffectId.EffectSubChance), Identifier(EffectId.EffectSubIntelligence),
         Identifier(EffectId.EffectSubStrength), Identifier(EffectId.EffectSubWisdom), Identifier(EffectId.EffectSubVitality),
         Identifier(EffectId.EffectSubRange), Identifier(EffectId.EffectSubRange135), Identifier(EffectId.EffectSubCriticalHit),
         Identifier(EffectId.EffectSubDamageBonus), Identifier(EffectId.EffectSubDamageBonusPercent), Identifier(EffectId.EffectSubDodgeAPProbability),
         Identifier(EffectId.EffectSubDodgeMPProbability), Identifier(EffectId.EffectSubHealBonus), Identifier(EffectId.EffectSubNeutralResistPercent),
         Identifier(EffectId.EffectSubEarthResistPercent), Identifier(EffectId.EffectSubWaterResistPercent), Identifier(EffectId.EffectSubAirResistPercent),
         Identifier(EffectId.EffectSubFireResistPercent), Identifier(EffectId.EffectSubAP), Identifier(EffectId.EffectLostAP),
         Identifier(EffectId.EffectSubMP), Identifier(EffectId.EffectLostMP), Identifier(EffectId.EffectSubPhysicalDamageReduction),
         Identifier(EffectId.EffectSubMagicDamageReduction)]
    public sealed class StatDebuffEffectHandler : SpellEffectHandler
    {
        public StatDebuffEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            var stat = GetEffectStat(Effect.Id);
            if (stat is not null)
            {
                var effectInteger = GenerateEffect();
                if (effectInteger is not null)
                {
                    if (stat is Stat.ActionPoints or Stat.MovementPoints)
                    {
                        var subEffect = stat is Stat.ActionPoints ? EffectId.EffectSubAP : EffectId.EffectSubMP;
                        var value = Effect.Id == subEffect ? effectInteger.Value :
                            FightFormulas.RollLooseStat(Cast.Caster, fighter, effectInteger.Value, Effect.Id is EffectId.EffectLostAP);
                        var dodged = (short)((effectInteger.Value > fighter.Stats[stat.Value].Total ? fighter.Stats[stat.Value].Total : effectInteger.Value) - value);

                        if (dodged is not 0)
                            fighter.Team!.Fight.Send(FightHandler.SendGameActionFightDodgePointLossMessage, new object[] { Cast.Caster, fighter, dodged, stat is Stat.ActionPoints });

                        if (stat is Stat.ActionPoints)
                            fighter.Trigger(BuffTriggerType.OnAPAttack, Cast.Caster);
                        else
                            fighter.Trigger(BuffTriggerType.OnMPAttack, Cast.Caster);

                        if (value > 0)
                        {
                            if ((Effect.Duration > 0 || fighter.Id == Cast.Caster.Id) && Effect.Id == subEffect)
                                fighter.AddStatBuff(this, (short)-value, stat.Value);
                            else if (stat is Stat.ActionPoints)
                                fighter.LostAP(value, Cast.Caster);
                            else
                                fighter.LostMP(value, Cast.Caster);

                            if (stat is Stat.ActionPoints)
                                fighter.Trigger(BuffTriggerType.OnAPLost, Cast.Caster);
                            else
                                fighter.Trigger(BuffTriggerType.OnMPLost, Cast.Caster);
                        }
                    }
                    else
                        fighter.AddStatBuff(this, (short)-effectInteger.Value, stat.Value);
                }
            }
            else
                Logger.Instance.LogWarn($"{Effect.Id} hasn't a binded stat...");
        }

        private static Stat? GetEffectStat(EffectId effect) =>
            effect switch
            {
                EffectId.EffectSubAgility => Stat.Agility,
                EffectId.EffectSubChance => Stat.Chance,
                EffectId.EffectSubIntelligence => Stat.Intelligence,
                EffectId.EffectSubStrength => Stat.Strength,
                EffectId.EffectSubWisdom => Stat.Wisdom,
                EffectId.EffectSubVitality => Stat.Vitality,
                EffectId.EffectSubRange or EffectId.EffectSubRange135 => Stat.Range,
                EffectId.EffectSubCriticalHit => Stat.CriticalHit,
                EffectId.EffectSubDamageBonus => Stat.AllDamagesBonus,
                EffectId.EffectSubDamageBonusPercent => Stat.DamagesBonusPercent,
                EffectId.EffectSubDodgeAPProbability => Stat.DodgeApLostProbability,
                EffectId.EffectSubDodgeMPProbability => Stat.DodgeMpLostProbability,
                EffectId.EffectSubHealBonus => Stat.HealBonus,
                EffectId.EffectSubNeutralResistPercent => Stat.NeutralElementResistPercent,
                EffectId.EffectSubEarthResistPercent => Stat.EarthElementResistPercent,
                EffectId.EffectSubWaterResistPercent => Stat.WaterElementResistPercent,
                EffectId.EffectSubAirResistPercent => Stat.AirElementResistPercent,
                EffectId.EffectSubFireResistPercent => Stat.FireElementResistPercent,
                EffectId.EffectSubAP or EffectId.EffectLostAP => Stat.ActionPoints,
                EffectId.EffectSubMP or EffectId.EffectLostMP => Stat.MovementPoints,
                EffectId.EffectSubMagicDamageReduction => Stat.MagicalReduction,
                EffectId.EffectSubPhysicalDamageReduction => Stat.PhysicalReduction,
                _ => default
            };
    }
}
