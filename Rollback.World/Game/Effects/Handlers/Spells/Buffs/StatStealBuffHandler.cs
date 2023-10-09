using Rollback.Common.DesignPattern.Attributes;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Fights;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Handlers.Fights;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectStealAgility), Identifier(EffectId.EffectStealAP440), Identifier(EffectId.EffectStealAP84),
        Identifier(EffectId.EffectStealChance), Identifier(EffectId.EffectStealIntelligence), Identifier(EffectId.EffectStealMP441),
        Identifier(EffectId.EffectStealMP77), Identifier(EffectId.EffectStealRange), Identifier(EffectId.EffectStealStrength),
        Identifier(EffectId.EffectStealVitality), Identifier(EffectId.EffectStealWisdom)]
    public sealed class StatStealBuffHandler : SpellEffectHandler
    {
        public StatStealBuffHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) { }

        protected override void InternalApply(FightActor fighter)
        {
            var stat = GetEffectStat(Effect.Id);
            if (stat is not null)
            {
                var displayedEffects = GetDisplayedEffects(Effect.Id);
                if (displayedEffects.Length is 2)
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
                                fighter.AddStatBuff(this, (short)-value, stat.Value, (short)displayedEffects[1]);

                                if (Effect.Duration > 0)
                                    Cast.Caster.AddStatBuff(this, value, stat.Value, (short)displayedEffects[0]);
                                else if (stat is Stat.ActionPoints)
                                    Cast.Caster.RegainAP(value);
                                else
                                    Cast.Caster.RegainMP(value);

                                if (stat is Stat.ActionPoints)
                                    fighter.Trigger(BuffTriggerType.OnAPLost, Cast.Caster);
                                else
                                    fighter.Trigger(BuffTriggerType.OnMPLost, Cast.Caster);
                            }
                        }
                        else
                        {
                            fighter.AddStatBuff(this, (short)-effectInteger.Value, stat.Value, (short)displayedEffects[1]);
                            Cast.Caster.AddStatBuff(this, effectInteger.Value, stat.Value, (short)displayedEffects[0]);
                        }
                    }
                }
            }
        }

        private static Stat? GetEffectStat(EffectId effect) =>
             effect switch
             {
                 EffectId.EffectStealAgility => Stat.Agility,
                 EffectId.EffectStealAP440 or EffectId.EffectStealAP84 => Stat.ActionPoints,
                 EffectId.EffectStealMP441 or EffectId.EffectStealMP77 => Stat.MovementPoints,
                 EffectId.EffectStealChance => Stat.Chance,
                 EffectId.EffectStealIntelligence => Stat.Intelligence,
                 EffectId.EffectStealRange => Stat.Range,
                 EffectId.EffectStealStrength => Stat.Strength,
                 EffectId.EffectStealVitality => Stat.Vitality,
                 EffectId.EffectStealWisdom => Stat.Wisdom,
                 _ => default
             };

        private static EffectId[] GetDisplayedEffects(EffectId effect) =>
            effect switch
            {
                EffectId.EffectStealAgility => new[] { EffectId.EffectAddAgility, EffectId.EffectSubAgility },
                EffectId.EffectStealAP440 => new[] { EffectId.EffectAddAP111, EffectId.EffectSubAP },
                EffectId.EffectStealAP84 => new[] { EffectId.EffectAddAP111, EffectId.EffectLostAP },
                EffectId.EffectStealChance => new[] { EffectId.EffectAddChance, EffectId.EffectSubChance },
                EffectId.EffectStealIntelligence => new[] { EffectId.EffectAddIntelligence, EffectId.EffectSubIntelligence },
                EffectId.EffectStealMP441 => new[] { EffectId.EffectAddMP, EffectId.EffectSubMP },
                EffectId.EffectStealMP77 => new[] { EffectId.EffectAddMP, EffectId.EffectLostMP },
                EffectId.EffectStealRange => new[] { EffectId.EffectAddRange, EffectId.EffectSubRange },
                EffectId.EffectStealStrength => new[] { EffectId.EffectAddStrength, EffectId.EffectSubStrength },
                EffectId.EffectStealVitality => new[] { EffectId.EffectAddVitality, EffectId.EffectSubVitality },
                EffectId.EffectStealWisdom => new[] { EffectId.EffectAddWisdom, EffectId.EffectSubWisdom },
                _ => Array.Empty<EffectId>()
            };
    }
}
