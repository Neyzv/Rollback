using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.Fights.Buffs.Types;
using Rollback.World.Game.Fights.Fighters;
using Rollback.World.Game.Fights.Results;
using Rollback.World.Game.Spells;
using Rollback.World.Game.Stats.Fields;
using Rollback.World.Game.World.Maps;
using Rollback.World.Game.World.Maps.CellsZone;
using Rollback.World.Game.World.Maps.CellsZone.Shapes;

namespace Rollback.World.Game.Fights
{
    public static class FightFormulas
    {
        public const byte UnlimitedZoneSize = 50;
        private const byte MaxErosionAmount = 50;

        /// <summary>
        /// Get the escape chance of the fighter from 0 to 1 and more
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="tacklers"></param>
        /// <returns></returns>
        public static double GetEscapeChance(FightActor fighter, FightActor[] tacklers)
        {
            var fighterCoeff = fighter.Stats[Stat.Agility].Total / 10;
            return 3 * (fighterCoeff + 2) / (fighterCoeff + tacklers.Sum(x => x.Stats[Stat.Agility].Total / 10) + 4) - 1;
        }

        public static FightSpellCastCriticalEnum RollCritical(FightActor fighter, Spell spell)
        {
            var result = FightSpellCastCriticalEnum.NORMAL;

            var criticalFailure = spell.CriticalFailureProbability - fighter.Stats[Stat.CriticalMiss].Total;
            var criticalHit = spell.CriticalHitProbability - fighter.Stats[Stat.CriticalHit].Total;

            if (spell.CriticalFailureProbability > 0 && Random.Shared.NextDouble() < 1 / (criticalFailure > 1 ? criticalFailure : 1))
                result = FightSpellCastCriticalEnum.CRITICAL_FAIL;
            else if (spell.CriticalHitProbability > 0 && Random.Shared.NextDouble() < 1 / (criticalHit > 1 ? criticalHit : 1) * 2.9901 / Math.Log(fighter.Stats[Stat.Agility].Total + 12))
                result = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return result;
        }

        public static void CalculateDamage(FightActor caster, Damage damage) // TO DO Weapon cf docu
        {
            short phyMagBonus = 0;
            short? statBonus = null;

            switch (damage.School)
            {
                case EffectSchool.Neutral:
                case EffectSchool.Earth:
                    statBonus = caster.Stats[Stat.Strength].Total;
                    phyMagBonus = caster.Stats[Stat.PhysicalDamage].Total;
                    break;

                case EffectSchool.Water:
                    statBonus = caster.Stats[Stat.Chance].Total;
                    phyMagBonus = caster.Stats[Stat.MagicalDamage].Total;
                    break;

                case EffectSchool.Air:
                    statBonus = caster.Stats[Stat.Agility].Total;
                    phyMagBonus = caster.Stats[Stat.MagicalDamage].Total;
                    break;

                case EffectSchool.Fire:
                    statBonus = caster.Stats[Stat.Intelligence].Total;
                    phyMagBonus = caster.Stats[Stat.MagicalDamage].Total;
                    break;
            }

            if (damage.Spell is not null)
                damage.Amount += (short)caster.GetBuffs<SpellBuff>(x => x.SpellId == damage.Spell.Id && x.Type is CharacterSpellModificationType.BaseDamage).Sum(x => x.Amount);

            if (statBonus.HasValue)
                damage.Amount = (short)((damage.Amount * ((100 + statBonus + caster.Stats[Stat.DamagesBonusPercent].Total) / 100d) +
                    caster.Stats[Stat.AllDamagesBonus].Total + phyMagBonus) * (caster.Stats[Stat.PermanentDamagePercent].Total > 1 ? caster.Stats[Stat.PermanentDamagePercent].Total : 1));

            if (damage.Amount < 0)
                damage.Amount = 0;
        }

        public static void CalculateDamageResistance(FightActor target, Damage damage, bool pvp)
        {
            short? resistancePercentPvP = null;
            short resistancePvP = 0;
            short resistancePercent = 0;
            short resistance = 0;
            short phyMagBonus = 0;

            switch (damage.School)
            {
                case EffectSchool.Neutral:
                    resistancePercentPvP = target.Stats[Stat.PvpNeutralElementResistPercent].Total;
                    resistancePvP = target.Stats[Stat.PvpNeutralElementReduction].Total;
                    resistancePercent = target.Stats[Stat.NeutralElementResistPercent].Total;
                    resistance = target.Stats[Stat.NeutralElementReduction].Total;
                    phyMagBonus = target.Stats[Stat.PhysicalReduction].Total;
                    break;

                case EffectSchool.Earth:
                    resistancePercentPvP = target.Stats[Stat.PvpEarthElementResistPercent].Total;
                    resistancePvP = target.Stats[Stat.PvpEarthElementReduction].Total;
                    resistancePercent = target.Stats[Stat.EarthElementResistPercent].Total;
                    resistance = target.Stats[Stat.EarthElementReduction].Total;
                    phyMagBonus = target.Stats[Stat.PhysicalReduction].Total;
                    break;

                case EffectSchool.Water:
                    resistancePercentPvP = target.Stats[Stat.PvpWaterElementResistPercent].Total;
                    resistancePvP = target.Stats[Stat.PvpWaterElementReduction].Total;
                    resistancePercent = target.Stats[Stat.WaterElementResistPercent].Total;
                    resistance = target.Stats[Stat.WaterElementReduction].Total;
                    phyMagBonus = target.Stats[Stat.MagicalReduction].Total;
                    break;

                case EffectSchool.Air:
                    resistancePercentPvP = target.Stats[Stat.PvpAirElementResistPercent].Total;
                    resistancePvP = target.Stats[Stat.PvpAirElementReduction].Total;
                    resistancePercent = target.Stats[Stat.AirElementResistPercent].Total;
                    resistance = target.Stats[Stat.AirElementReduction].Total;
                    phyMagBonus = target.Stats[Stat.MagicalReduction].Total;
                    break;

                case EffectSchool.Fire:
                    resistancePercentPvP = target.Stats[Stat.PvpFireElementResistPercent].Total;
                    resistancePvP = target.Stats[Stat.PvpFireElementReduction].Total;
                    resistancePercent = target.Stats[Stat.FireElementResistPercent].Total;
                    resistance = target.Stats[Stat.FireElementReduction].Total;
                    phyMagBonus = target.Stats[Stat.MagicalReduction].Total;
                    break;
            }

            if (resistancePercentPvP.HasValue)
                damage.Amount = (short)(damage.Amount * (1.0d - (resistancePercent + (pvp ? resistancePercentPvP : 0)) / 100d) - resistance - phyMagBonus - (pvp ? resistancePvP : 0));

            if (damage.Amount < 0)
                damage.Amount = 0;
        }

        public static short CalculateZoneEfficiency(MapPoint castPoint, MapPoint targetedPoint, Zone zone, short amount)
        {
            if (zone.Radius < UnlimitedZoneSize && zone is not World.Maps.CellsZone.Shapes.Single)
            {
                uint distance;

                if (zone is Cross cross && cross.Diagonal)
                    distance = targetedPoint.ManhattanDistanceTo(castPoint) / 2;
                else
                    distance = targetedPoint.ManhattanDistanceTo(castPoint);

                if (distance <= zone.Radius)
                    amount = (short)Math.Floor(amount * Math.Max(0d, 1 - 0.10d * Math.Min(distance, 4)));
            }

            return amount;
        }

        public static short CalculateHeal(FightActor caster, short amount) =>
            (short)(amount * (100 + caster.Stats[Stat.Intelligence].Total) / 100d + caster.Stats[Stat.HealBonus].Total);

        public static short CalculateErodedHealth(FightActor target, short damageAmount) =>
            (short)Math.Floor(damageAmount * ((target.Stats[Stat.Erosion].Total > MaxErosionAmount ? MaxErosionAmount : target.Stats[Stat.Erosion].Total) / 100d));

        public static short RollLooseStat(FightActor from, FightActor target, short amount, bool ap)
        {
            short result = 0;
            var field = (UseablePointsField)target.Stats[ap ? Stat.ActionPoints : Stat.MovementPoints];

            for (var i = 0; i < amount && i < field.Total; i++)
            {
                var probability = 50 * from.Stats[Stat.Wisdom].Total / 10 /
                    (target.Stats[ap ? Stat.DodgeApLostProbability : Stat.DodgeMpLostProbability].Total > 0 ? target.Stats[ap ? Stat.DodgeApLostProbability : Stat.DodgeMpLostProbability].Total : 1)
                    * (field.Total - result) / field.TotalMax;

                if (probability > 90)
                    probability = 90;
                else if (probability < 10)
                    probability = 10;

                if (Random.Shared.Next(101) < probability)
                    result++;
            }

            return result;
        }

        public static short CalculatePushBackDamages(FightActor from, short range, byte targets) =>
            (short)((Random.Shared.NextDouble() * (1.8d - 1d) + 1d) * ((from is SummonedFighter summon ? summon.Summoner : from).Level / 2 + 32) * range / (4 * Math.Pow(2, targets)));

        public static short CalculateArmorReduction(FightActor target, short reduction) =>
            (short)(reduction * (100 + 5 * (target.Level > 200 ? 200 : target.Level)) / 100d);

        public static short CalculateDamageReflection(FightActor target, short amount)
        {
            var reflectedDamages = target.Stats[Stat.Reflect].Context * (1 + target.Stats[Stat.Wisdom].Total / 100d) +
                target.Stats[Stat.Reflect].TotalWithOutContext;

            if (reflectedDamages > amount / 2d)
                reflectedDamages = amount / 2d;

            return (short)Math.Floor(reflectedDamages);
        }

        public static int CalculateEarnedExperience(IFightResult loot, CharacterFighter[] allies, MonsterFighter[] ennemies, short bonus,
            bool win, bool useRate = true)
        {
            var result = 0;

            if (allies.Length is not 0 && allies[0].Team!.Fight.Winners.Count is not 0)
            {
                var highestEnnemyLevel = ennemies.Max(x => x.Level);

                double levelDifferenceMultiplicator;
                if (win)
                    levelDifferenceMultiplicator = allies.Where(x => x.Level >= highestEnnemyLevel / 3).Count() switch
                    {
                        1 => 1,
                        2 => 1.1,
                        3 => 1.5,
                        4 => 2.3,
                        5 => 3.1,
                        6 => 3.6,
                        7 => 4.2,
                        8 => 4.7,
                        _ => 0.5d
                    };
                else
                    levelDifferenceMultiplicator = 0.5d;

                var sumAlliesLevel = allies.Sum(x => x.Level);
                var sumEnnemiesLevel = ennemies.Sum(x => x.Level);

                var levelCoeff = 1d;
                if (sumAlliesLevel - 5 > sumEnnemiesLevel)
                    levelCoeff = sumEnnemiesLevel / (double)sumAlliesLevel;
                else if (sumAlliesLevel + 10 < sumEnnemiesLevel)
                    levelCoeff = (sumAlliesLevel + 10) / (double)sumAlliesLevel;

                result = (int)((1 + ((loot.Wisdom + bonus) / 100d)) * (levelDifferenceMultiplicator + levelCoeff)
                    * (ennemies.Where(x => !x.Alive).Sum(x => x.XP) / (double)allies.Length) * (useRate ? GeneralConfiguration.Instance.XPRate : 1));
            }

            return result;
        }

        public static int CalculateEarnedKamas(int baseKamas, short bonus, bool useRate = true) =>
            (int)(baseKamas * (bonus <= 0 ? 1 : bonus / 5d / 100) * (useRate ? GeneralConfiguration.Instance.KamasRate : 1));

        public static Dictionary<short, short> RollDrop(IFightResult loot, IEnumerable<MonsterDropRecord> potentialsDrops, ref Dictionary<short, short> alreadyDroppedItems,
            bool useRate = true)
        {
            var result = new Dictionary<short, short>();

            foreach (var item in potentialsDrops)
            {
                if (Random.Shared.Next(0, 101) * loot.Prospecting / 100d <= item.Percentage * (useRate ? GeneralConfiguration.Instance.DropRate : 1))
                {
                    alreadyDroppedItems.TryGetValue(item.ItemId, out short alreadyEarned);

                    short amount = (short)Random.Shared.Next(1, item.MaxAmountPerMonster + 1);
                    if (alreadyEarned + amount > item.MaxAmountPerFight)
                        amount = (short)(item.MaxAmountPerFight - amount);

                    if (amount > 0)
                    {
                        if (result.ContainsKey(item.ItemId))
                            result[item.ItemId] += amount;
                        else
                            result[item.ItemId] = amount;

                        if (alreadyDroppedItems.ContainsKey(item.ItemId))
                            alreadyDroppedItems[item.ItemId] += amount;
                        else
                            alreadyDroppedItems[item.ItemId] = amount;
                    }
                }
            }

            return result;
        }
    }
}
