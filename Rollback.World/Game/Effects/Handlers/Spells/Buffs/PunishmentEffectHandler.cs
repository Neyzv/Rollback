using Rollback.Common.DesignPattern.Attributes;
using Rollback.Common.Logging;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Fights;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.World.Maps.CellsZone;

namespace Rollback.World.Game.Effects.Handlers.Spells.Buffs
{
    [Identifier(EffectId.EffectPunishment)]
    public sealed class PunishmentEffectHandler : SpellEffectHandler
    {
        private readonly Dictionary<short, short> _buffs;

        public PunishmentEffectHandler(EffectBase effect, List<FightActor> target, SpellCast cast, Zone zone) : base(effect, target, cast, zone) =>
            _buffs = new();

        protected override void InternalApply(FightActor fighter) =>
            fighter.AddTriggerBuff(BuffTriggerType.AfterDamaged, this, OnAfterDamaged);

        private void OnAfterDamaged(TriggerBuff buff, FightActor trigger, BuffTriggerType type, object? token)
        {
            if (buff.Handler.Effect is EffectDice effectDice && token is Damage damage && damage.Amount > 0)
            {
                var boostType = GetPunishmentStat(effectDice.DiceNum);
                if (boostType is not null)
                {
                    if (!_buffs.ContainsKey(buff.Target.Team!.Fight.RoundNumber))
                        _buffs[buff.Target.Team.Fight.RoundNumber] = 0;

                    short actualBoost = _buffs[buff.Target.Team.Fight.RoundNumber];
                    if (actualBoost < effectDice.DiceFace)
                    {
                        short amount = actualBoost + damage.Amount > effectDice.DiceFace ? (short)(effectDice.DiceFace - actualBoost) : damage.Amount;

                        _buffs[buff.Target.Team.Fight.RoundNumber] += amount;

                        if (boostType is Stat.Health)
                            buff.Target.Heal(buff.Target, amount, applyBoost: false);
                        else
                            buff.Target.AddStatBuff(this, amount, boostType.Value, effectDice.DiceNum);
                    }
                }
                else
                    Logger.Instance.LogWarn($"Uknown stat field for punishment with action identifier {effectDice.DiceNum}...");
            }
        }

        private static Stat? GetPunishmentStat(short punishmentAction) =>
            (Actions)punishmentAction switch
            {
                Actions.ActionCharacterBoostAgility => Stat.Agility,
                Actions.ActionCharacterBoostStrength => Stat.Strength,
                Actions.ActionCharacterBoostIntelligence => Stat.Intelligence,
                Actions.ActionCharacterBoostChance => Stat.Chance,
                Actions.ActionCharacterBoostWisdom => Stat.Wisdom,
                Actions.ActionCharacterBoostDamagesPercent => Stat.DamagesBonusPercent,
                Actions.ActionCharacterBoostVitality => Stat.Vitality,
                Actions.ActionCharacterLifePointsWin or (Actions)407 /* IDK */ => Stat.Health,
                _ => default
            };
    }
}
