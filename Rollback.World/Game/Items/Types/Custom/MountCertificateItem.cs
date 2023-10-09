using Rollback.Protocol.Types;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Mounts;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemType((short)ItemType.CertificatDeDragodinde)]
    public sealed class MountCertificateItem : PlayerItem
    {
        private const byte ValidityDaysCount = 60;

        private Mount? _mount;
        public Mount? Mount
        {
            get
            {
                var mount = MountEffect is null ? default : _mount ??= MountManager.Instance.GetMountById(MountEffect!.MountId);

                if (mount is null)
                    InvalidCertificate();
                else
                    mount.SetOwner(_storage.Owner);

                return mount;
            }
        }

        private EffectMount? _mountEffect;
        private EffectMount? MountEffect =>
            _mountEffect ??= Effects.OfType<EffectMount>().FirstOrDefault();

        private EffectString? _belongsToEffect;
        private EffectString? BelongsToEffect =>
            _belongsToEffect ??= Effects.OfType<EffectString>().FirstOrDefault(x => x.Id is EffectId.EffectBelongsTo);

        private EffectDuration? _validityEffect;
        private EffectDuration? ValidityEffect =>
            _validityEffect ??= Effects.OfType<EffectDuration>().FirstOrDefault(x => x.Id is EffectId.EffectValidity);

        public override ObjectItem ObjectItem
        {
            get
            {
                UpdateValidity();

                return base.ObjectItem;
            }
        }

        public MountCertificateItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory)
        {
            Created += OnCertificateCreated;
            Deleted += _ => InvalidCertificate();
            
            UpdateValidity();
        }

        private void OnCertificateCreated(PlayerItem item)
        {
            if (Mount is null)
                if (MountManager.Instance.GetMountRecord(x => x.CertificateId == Id) is { } mountTemplate)
                    _mount = MountManager.Instance.CreateMount(_storage.Owner, mountTemplate);
                else
                {
                    InvalidCertificate();
                    
                    return;
                }
            
            Effects.Clear();
            
            _mountEffect = new EffectMount(_mount!.Id, DateTimeOffset.Now, _mount.ModelId);
            Effects.Add(_mountEffect);
            
            _belongsToEffect = new EffectString(EffectId.EffectBelongsTo, _storage.Owner.Name);
            Effects.Add(_belongsToEffect);

            _validityEffect = new EffectDuration(EffectId.EffectValidity, TimeSpan.FromDays(ValidityDaysCount));
            Effects.Add(_validityEffect);
        }

        private void UpdateValidity()
        {
            if (ValidityEffect is not null && Mount is not null)
            {
                var validity = DateTimeOffset.FromUnixTimeMilliseconds((long)MountEffect!.ExpirationDate) + TimeSpan.FromDays(ValidityDaysCount) - DateTime.Now;

                if (validity > TimeSpan.Zero)
                    ValidityEffect.SetDuration(validity);
                else
                    InvalidCertificate();
            }
        }

        private void InvalidCertificate()
        {
            if (_mount is not null)
                MountManager.Instance.DeleteMount(_mount.Id);

            Effects.Clear();

            Effects.Add(new EffectInteger()
            {
                Id = EffectId.EffectInvalidCertificate
            });
        }
    }
}
