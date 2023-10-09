using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;

namespace Rollback.World.Database.Spells
{
    public static class SpellLevelRelator
    {
        public const string GetSpellLevelById = "SELECT * FROM spells_levels WHERE Id = {0}";
    }

    [Table("spells_levels")]
    public sealed record SpellLevelRecord
    {
        public SpellLevelRecord()
        {
            _binaryEffects = Array.Empty<byte>();
            _binaryCriticalEffects = Array.Empty<byte>();

            Effects = Array.Empty<EffectDice>();
            CriticalEffects = Array.Empty<EffectDice>();

            _statesRequiredCSV = string.Empty;
            StatesRequired = new();

            _statesForbiddenCSV = string.Empty;
            StatesForbidden = new();
        }

        [Key]
        public int Id { get; set; }

        public byte APCost { get; set; }

        public sbyte MinRange { get; set; }

        public sbyte MaxRange { get; set; }

        public bool CastInLine { get; set; }

        public bool CastTestLOS { get; set; }

        public bool NeedFreeCell { get; set; }

        public bool RangeCanBeBoosted { get; set; }

        public bool CriticalFailureEndsTurn { get; set; }

        public sbyte CriticalHitProbability { get; set; }

        public sbyte CriticalFailureProbability { get; set; }

        public byte MaxCastPerTurn { get; set; }

        public byte MaxCastPerTarget { get; set; }

        public byte MinCastInterval { get; set; }

        public byte MinPlayerLevel { get; set; }

        private byte[] _binaryEffects;
        public byte[] BinaryEffects
        {
            get => _binaryEffects;
            set
            {
                _binaryEffects = value;
                try
                {
                    Effects = EffectManager.DeserializeEffects(value).Select(x => (EffectDice)x).ToArray();
                }
                catch
                {
                    Logger.Instance.LogError(msg: $"Incorrect datas format while deserializing effects of spell level {Id}...");
                }
            }
        }

        [Ignore]
        public EffectDice[] Effects { get; private set; }

        private byte[] _binaryCriticalEffects;
        public byte[] BinaryCriticalEffects
        {
            get => _binaryCriticalEffects;
            set
            {
                _binaryCriticalEffects = value;
                try
                {
                    CriticalEffects = EffectManager.DeserializeEffects(value).Select(x => (EffectDice)x).ToArray();
                }
                catch
                {
                    Logger.Instance.LogError(msg: $"Incorrect datas format while deserializing critical effects of spell level {Id}...");
                }
            }
        }

        [Ignore]
        public EffectDice[] CriticalEffects { get; private set; }

        private string _statesRequiredCSV;
        public string StatesRequiredCSV
        {
            get => _statesRequiredCSV;
            set
            {
                _statesRequiredCSV = value;

                StatesRequired.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (var state in value.Split(','))
                        if (short.TryParse(state, out var stateId))
                        {
                            if (!StatesRequired.Contains(stateId))
                                StatesRequired.Add(stateId);
                            else
                                Logger.Instance.LogError(msg: $"Can not add required state {state}, the state was already added, for spell level {Id}...");
                        }
                        else
                            Logger.Instance.LogError(msg: $"Can not convert required state {state} to short value, for spell level {Id}...");
                }
            }
        }

        [Ignore]
        public HashSet<short> StatesRequired { get; private set; }

        private string _statesForbiddenCSV;
        public string StatesForbiddenCSV
        {
            get => _statesForbiddenCSV;
            set
            {
                _statesForbiddenCSV = value;

                StatesForbidden.Clear();

                if (!string.IsNullOrEmpty(value))
                {
                    foreach (var state in value.Split(','))
                        if (short.TryParse(state, out var stateId))
                        {
                            if (!StatesForbidden.Contains(stateId))
                                StatesForbidden.Add(stateId);
                            else
                                Logger.Instance.LogError(msg: $"Can not add forbidden state {state}, the state was already added, for spell level {Id}...");
                        }
                        else
                            Logger.Instance.LogError(msg: $"Can not convert forbidden state {state} to short value, for spell level {Id}...");
                }
            }
        }

        [Ignore]
        public HashSet<short> StatesForbidden { get; private set; }
    }
}
