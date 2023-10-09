using Rollback.Common.Extensions;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;

namespace Rollback.World.Game.Items.Types.Custom
{
    [ItemId(KeyringId)]
    public sealed class KeyringItem : PlayerItem
    {
        public const short KeyringId = 10207;

        public KeyringItem(CharacterItemRecord record, Inventory inventory)
            : base(record, inventory)
        {
            Created += _ => Reset();
            CheckResetNeeded();
        }

        private void CheckResetNeeded()
        {
            if (Effects.FirstOrDefault(x => x.Id is EffectId.EffectReceiveOn) is { } effect
                && effect is EffectDate dateEffect && !dateEffect.Date.IsSameWeek(DateTime.Now))
                Reset();
        }

        private EffectInteger? GetEffectByKeyId(short keyId)
        {
            var effectToDelete = default(EffectInteger);

            foreach (var effect in Effects)
                if (effect.Id == EffectId.EffectItemName && effect is EffectInteger effectInteger &&
                    effectInteger.Value == keyId)
                {
                    effectToDelete = effectInteger;
                    break;
                }

            return effectToDelete;
        }

        public void Reset()
        {
            Effects.Clear();

            Effects.Add(new EffectDate(EffectId.EffectReceiveOn, DateTime.Now));

            foreach (var key in ItemManager.Instance.GetTemplateRecords(x => x.TypeId is ItemType.Clef))
                Effects.Add(new EffectInteger()
                {
                    Id = EffectId.EffectItemName,
                    Value = key.Id
                });
        }

        public bool CanUseKey(short keyId) =>
            GetEffectByKeyId(keyId) is not null;

        public bool TryUseKey(short keyId) =>
            GetEffectByKeyId(keyId) is { } effectToDelete && Effects.Remove(effectToDelete);
    }
}
