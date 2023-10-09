using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Database.Monsters;
using Rollback.World.Game.Guilds;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.RolePlayActors.TaxCollectors;
using Rollback.World.Game.Stats.Fields;

namespace Rollback.World.Game.Stats
{
    public sealed class StatsData
    {
        private const byte CharacterBaseHealth = 50;

        private readonly Dictionary<Stat, StatField> _stats;
        public StatField this[Stat stat]
        {
            get
            {
                if (!_stats.ContainsKey(stat))
                    _stats[stat] = StatManager.Instance.CreateStatField(stat, this);

                return _stats[stat];
            }
        }

        public Character? Owner { get; }

        public HealthField Health { get; }

        public UseablePointsField AP =>
            (UseablePointsField)this[Stat.ActionPoints];

        public UseablePointsField MP =>
            (UseablePointsField)this[Stat.MovementPoints];

        private StatsData(Character owner, int baseMaxHealth, int actualHealth, short ap, short mp,
            short vitality, short wisdom, short strength, short chance, short agility, short intelligence)
            : this(baseMaxHealth, actualHealth, ap, mp, vitality, wisdom, strength, chance, agility, intelligence) =>
            Owner = owner;

        private StatsData(int baseMaxHealth, int actualHealth, short ap, short mp,
            short vitality, short wisdom, short strength, short chance, short agility, short intelligence)
        {
            _stats = new();

            Health = new(this, baseMaxHealth, actualHealth);

            this[Stat.ActionPoints].Base = ap;
            this[Stat.MovementPoints].Base = mp;

            this[Stat.SummonableCreaturesBoost].Base = 1;

            this[Stat.Vitality].Base = vitality;
            this[Stat.Wisdom].Base = wisdom;
            this[Stat.Strength].Base = strength;
            this[Stat.Chance].Base = chance;
            this[Stat.Agility].Base = agility;
            this[Stat.Intelligence].Base = intelligence;
        }

        public static StatsData CreateStats(Character owner, CharacterRecord record, byte level)
        {
            var result = new StatsData(owner, CharacterBaseHealth + (level * 5), record.Health, record.AP, record.MP, record.Vitality, record.Wisdom,
                record.Strength, record.Chance, record.Agility, record.Intelligence);

            result[Stat.Vitality].Additional = record.PermanentVitality;
            result[Stat.Wisdom].Additional = record.PermanentWisdom;
            result[Stat.Strength].Additional = record.PermanentStrength;
            result[Stat.Chance].Additional = record.PermanentChance;
            result[Stat.Agility].Additional = record.PermanentAgility;
            result[Stat.Intelligence].Additional = record.PermanentIntelligence;

            return result;
        }

        public static StatsData CreateStats(MonsterGradeRecord record)
        {
            var result = new StatsData(record.Health, record.Health, record.AP, record.MP, 0, record.Wisdom,
                record.Strength, record.Chance, record.Agility, record.Intelligence);

            result[Stat.DodgeApLostProbability].Base = record.APDodge;
            result[Stat.DodgeMpLostProbability].Base = record.MPDodge;

            result[Stat.EarthElementResistPercent].Base = record.EarthResistance;
            result[Stat.AirElementResistPercent].Base = record.AirResistance;
            result[Stat.FireElementResistPercent].Base = record.FireResistance;
            result[Stat.WaterElementResistPercent].Base = record.WaterResistance;
            result[Stat.NeutralElementResistPercent].Base = record.NeutralResistance;

            return result;
        }

        public static StatsData CreateStats(Guild guild)
        {
            var stats = (short)Math.Floor(guild.Level * 2.5d);
            var othersStats = (short)Math.Floor(stats / 10d);

            var result = new StatsData(guild.TaxCollectorHealth, guild.TaxCollectorHealth,
                TaxCollectorManager.BaseAP, TaxCollectorManager.BaseMP, 0, guild.TaxCollectorWisdom,
                stats, stats, stats, stats);

            result[Stat.DodgeApLostProbability].Base = othersStats;
            result[Stat.DodgeMpLostProbability].Base = othersStats;

            result[Stat.AllDamagesBonus].Base = guild.TaxCollectorDamageBonuses;

            var resistance = (short)Math.Floor(guild.Level / 8d);
            result[Stat.EarthElementResistPercent].Base = resistance;
            result[Stat.AirElementResistPercent].Base = resistance;
            result[Stat.FireElementResistPercent].Base = resistance;
            result[Stat.WaterElementResistPercent].Base = resistance;
            result[Stat.NeutralElementResistPercent].Base = resistance;

            return result;
        }

        public StatsData Clone()
        {
            var stats = new StatsData(Health.BaseMax, Health.Actual, AP.Base, MP.Base, this[Stat.Vitality].Base, this[Stat.Wisdom].Base,
                this[Stat.Strength].Base, this[Stat.Chance].Base, this[Stat.Agility].Base, this[Stat.Intelligence].Base);

            foreach (var stat in _stats)
            {
                stats[stat.Key].Base = stat.Value.Base;
                stats[stat.Key].Additional = stat.Value.Additional;
                stats[stat.Key].Equipments = stat.Value.Equipments;
                stats[stat.Key].AlignmentBonus = stat.Value.AlignmentBonus;
            }

            return stats;
        }
    }
}
