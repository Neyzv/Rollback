using System.Text.RegularExpressions;
using Rollback.Common.ORM;
using Rollback.Protocol.Types;
using Rollback.World.Database.Mounts;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.Looks;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Mounts;

namespace Rollback.World.Game.Mounts
{
    public sealed class Mount
    {
        private static readonly Regex _nameRegex;
        
        private const byte MaxReproductionCount = 5;
        private const short StatMax = 10_000;
        private const byte MaxTiredness = 240;

        private readonly MountRecord _template;
        private readonly AccountMountRecord _record;
        private readonly List<EffectInteger> _effects;

        private Character? _owner;

        public int Id =>
            _record.Id;

        public int AccountId
        {
            get => _record.AccountId;
            private set => _record.AccountId = value;
        }

        public string Name
        {
            get => _record.Name;
            private set => _record.Name = value;
        }

        public short ModelId =>
            _record.ModelId;

        public bool Sex =>
            _record.Sex;

        public long Experience
        {
            get => _record.Experience;
            private set => _record.Experience = value;
        }

        public long UpperExperienceLevelFloor { get; private set; }

        public long LowerExperienceLevelFloor { get; private set; }

        public sbyte Level { get; private set; }

        public int MaxPods =>
            _template.BasePods + _template.PodsPerLevel * Level;

        public int Energy
        {
            get => _record.Energy;
            private set => _record.Energy = value > MaxEnergy ? MaxEnergy : value < 0 ? 0 : value;
        }

        public int MaxEnergy =>
            _template.BaseEnergy + _template.EnergyPerLevel * Level;

        public int Stamina
        {
            get => _record.Stamina;
            private set => _record.Stamina = value;
        }

        public int Maturity
        {
            get => _record.Stamina;
            private set => _record.Stamina = value;
        }

        public int? PaddockMapId
        {
            get => _record.PaddockMapId;
            set => _record.PaddockMapId = value;
        }

        public bool IsInStable
        {
            get => _record.IsInStable;
            set => _record.IsInStable = value;
        }

        public short CertificateId =>
            _template.CertificateId;

        public ActorLook Look =>
            _template.Look;

        public MountClientData MountClientData =>
            new(Sex,
                _template.BaseEnergy > 0,
                _template.BaseEnergy is 0,
                false,
                Id,
                ModelId,
                _record.Ancestors,
                _record.Behaviors.Select(x => (int)x).ToArray(),
                Name,
                _record.AccountId,
                Experience,
                LowerExperienceLevelFloor,
                UpperExperienceLevelFloor,
                Level,
                MaxPods,
                Stamina,
                StatMax,
                Maturity,
                _template.MaturityForAdult,
                Energy,
                MaxEnergy,
                _record.Serinity,
                -StatMax,
                StatMax,
                _record.Love,
                StatMax,
                0,
                _record.Tiredness,
                MaxTiredness,
                _record.ReproductionCount,
                MaxReproductionCount,
                _effects.Select(x => (ObjectEffectInteger)x.ObjectEffect).ToArray());

        static Mount() =>
            _nameRegex = new Regex(@"^(?=.{0,30}$)(?=[A-z0-9éèùîïüûôö]*$)", RegexOptions.Compiled);

        public Mount(MountRecord template, AccountMountRecord record)
        {
            _template = template;
            _record = record;

            RefreshLevelValues();

            _effects = new List<EffectInteger>();
            RefreshMountEffects();
        }

        private void RefreshMountEffects()
        {
            _effects.Clear();
            
            foreach (var effect in _template.Effects)
                if (effect is EffectInteger integerEffect)
                {
                    var effectPower = (short)Math.Floor(integerEffect.Value * Level / (double)ExperienceManager.Instance.MaxMountLevel);
                    if(effectPower > 0)
                        _effects.Add(new EffectInteger()
                        {
                            Id = integerEffect.Id,
                            Value = effectPower
                        });
                }
        }

        private void RefreshLevelValues()
        {
            Level = ExperienceManager.Instance.GetMountLevel(Experience);
            UpperExperienceLevelFloor = ExperienceManager.Instance.GetMountUpperExperienceLevelFloor(Experience);
            LowerExperienceLevelFloor = ExperienceManager.Instance.GetMountLowerExperienceLevelFloor(Experience);
        }

        public void HandleMountEffects(bool apply)
        {
            if (_owner is not null)
            {
                foreach(var effect in _effects)
                    EffectManager.Instance.HandleItemEffect(_owner, effect, apply);
            
                _owner.RefreshStats();
            }
        }

        private void AdjustLevel()
        {
            if (Level < ExperienceManager.Instance.MaxMountLevel)
            {
                RefreshLevelValues();

                if (_owner is not null)
                {
                    if (_owner.IsRiding && _owner.EquipedMount?.Id == Id)
                    {
                        HandleMountEffects(false);
                        RefreshMountEffects();
                        HandleMountEffects(true);
                    }                    
                    
                    MountHandler.SendMountSetMessage(_owner.Client);
                }
            }
        }

        public void SetOwner(Character owner)
        {
            _owner = owner;
            AccountId = _owner.Client.Account!.Id;
        }

        public void RenameMount(string name)
        {
            if (_owner is not null && _nameRegex.IsMatch(name))
            {
                Name = name;

                MountHandler.SendMountRenamedMessage(_owner.Client, this);
            }
        }

        public void ChangeExperience(long amount)
        {
            if (amount > 0 && (ulong)(Experience + amount) > long.MaxValue)
                Experience = long.MaxValue;
            else if (Experience + amount < 0)
                Experience = 0;
            else
                Experience += amount;

            if (Experience < LowerExperienceLevelFloor || (UpperExperienceLevelFloor > 0 && Experience >= UpperExperienceLevelFloor))
                AdjustLevel();
        }         

        public void Save() =>
            DatabaseAccessor.Instance.InsertOrUpdate(_record);

        public void Delete() =>
            DatabaseAccessor.Instance.Delete(_record);
    }
}
