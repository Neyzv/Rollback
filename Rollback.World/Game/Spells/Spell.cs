using Rollback.Common.Logging;
using Rollback.World.Database.Spells;
using Rollback.World.Game.Effects.Types;

namespace Rollback.World.Game.Spells
{
    public class Spell
    {
        private readonly SpellTemplateRecord _template;
        private SpellLevelRecord LevelRecord =>
            _template.SpellLevels[Level - 1];

        public short Id =>
            _template.Id;

        public sbyte Level { get; private set; }

        public byte MinPlayerLevel =>
            LevelRecord.MinPlayerLevel;

        public byte APCost =>
            LevelRecord.APCost;

        public bool NeedFreeCell =>
            LevelRecord.NeedFreeCell;

        public sbyte MinRange =>
            LevelRecord.MinRange;

        public sbyte MaxRange =>
            LevelRecord.MaxRange;

        public bool RangeCanBeBoosted =>
            LevelRecord.RangeCanBeBoosted;

        public bool CastInline =>
            LevelRecord.CastInLine;

        public bool CastTestLOS =>
            LevelRecord.CastTestLOS;

        public sbyte CriticalHitProbability =>
            LevelRecord.CriticalHitProbability;

        public sbyte CriticalFailureProbability =>
            LevelRecord.CriticalFailureProbability;

        public byte MinCastInterval =>
            LevelRecord.MinCastInterval;

        public byte MaxCastPerTurn =>
            LevelRecord.MaxCastPerTurn;

        public byte MaxCastPerTarget =>
            LevelRecord.MaxCastPerTarget;

        public IReadOnlyCollection<short> StatesRequired =>
            LevelRecord.StatesRequired;

        public IReadOnlyCollection<short> StatesForbidden =>
            LevelRecord.StatesForbidden;

        private EffectDice[]? _effects;
        public EffectDice[] Effects =>
            _effects ??= LevelRecord.Effects.ToArray();

        private EffectDice[]? _criticalEffects;
        public EffectDice[] CriticalEffects =>
            _criticalEffects ??= LevelRecord.CriticalEffects.ToArray();

        public Spell(SpellTemplateRecord template, sbyte level)
        {
            if (level <= 0 || level > template.SpellLevels.Length)
                Logger.Instance.LogError(msg: $"Can not create spell {template.Id} with level {level}...");

            _template = template;
            Level = level;
        }

        public virtual bool Upgrade()
        {
            if (Level < _template.SpellLevels.Length)
            {
                Level++;
                return true;
            }

            return false;
        }

        public bool DownGrade()
        {
            if (Level > 1)
            {
                Level--;

                return true;
            }

            return false;
        }
    }
}
