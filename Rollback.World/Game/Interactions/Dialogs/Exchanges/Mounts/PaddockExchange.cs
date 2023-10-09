using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Types.Custom;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Mounts;

namespace Rollback.World.Game.Interactions.Dialogs.Exchanges.Mounts
{
    public sealed class PaddockExchange : Exchange<Paddock>
    {
        private readonly HashSet<int> _availablesMounts = new ();
        
        public override ExchangeTypeEnum ExchangeType =>
            ExchangeTypeEnum.MOUNT_STABLE;

        public PaddockExchange(Character character, Paddock dialoger)
            : base(character, dialoger) { }

        public override void MoveItem(int actorId, int uid, int quantity) { }

        public override void SetKamas(int actorId, int amount) { }

        protected override void InternalOpen()
        {
            var stabledMounts = new List<Mount>();
            var paddockedMounts = new List<Mount>();

            foreach (var mount in MountManager.Instance.GetMounts(x => x.PaddockMapId.HasValue &&
                ((Dialoger.IsPublic && MountManager.Instance.GetPaddock(x.PaddockMapId.Value) is { IsPublic: true }) || x.PaddockMapId == Dialoger.MapId)
                && x.AccountId == Character.Client.Account!.Id))
            {
                if (mount.IsInStable)
                    stabledMounts.Add(mount);
                else
                    paddockedMounts.Add(mount);
                
                _availablesMounts.Add(mount.Id);
            }

            MountHandler.SendExchangeStartOkMountMessage(Character.Client, stabledMounts, paddockedMounts);
        }

        public void EquipToInventory(int mountId)
        {
            if (Character.EquipedMount is not null && Character.EquipedMount.Id == mountId &&
                Character.Inventory.AddItem(Character.EquipedMount.CertificateId, 1,
                    EffectGenerationType.Normal,
                    new[]
                    {
                        new EffectMount(Character.EquipedMount.Id, DateTimeOffset.Now, Character.EquipedMount.ModelId)
                    }) is not null)
                Character.UnEquipMount();
        }

        public void EquipToStable(int mountId)
        {
            if (Character.EquipedMount is not null && Character.EquipedMount.Id == mountId)
            {
                var mount = Character.EquipedMount;
                Character.UnEquipMount();
                mount.PaddockMapId = Dialoger.MapId;
                mount.IsInStable = true;
                
                MountHandler.SendExchangeMountStableAddMessage(Character.Client, mount);
                _availablesMounts.Add(mount.Id);
            }
        }

        public void InventoryToEquip(int uid)
        {
            if (Character.EquipedMount is not null)
                EquipToInventory(Character.EquipedMount.Id);

            if (Character.Inventory.GetItemByUID(uid) is MountCertificateItem { Mount: not null } certificate
                && Character.TryEquipMount(certificate.Mount))
                Character.Inventory.RemoveItem(certificate, certificate.Quantity);
        }

        public void InventoryToStable(int uid)
        {
            if (Character.Inventory.GetItemByUID(uid) is MountCertificateItem { Mount: not null } certificate)
            {
                Character.Inventory.RemoveItem(certificate, certificate.Quantity);

                certificate.Mount.PaddockMapId = Dialoger.MapId;
                certificate.Mount.IsInStable = true;
                
                MountHandler.SendExchangeMountStableAddMessage(Character.Client, certificate.Mount);

                _availablesMounts.Add(certificate.Mount.Id);
            }
        }

        public void StableToInventory(int mountId)
        {
            if (_availablesMounts.Remove(mountId) &&
                MountManager.Instance.GetMountById(mountId) is {} mount &&
                Character.Inventory.AddItem(mount.CertificateId, 1,
                    EffectGenerationType.Normal,
                    new[]
                    {
                        new EffectMount(mount.Id, DateTimeOffset.Now, mount.ModelId)
                    }) is not null)
            {
                mount.PaddockMapId = null;
                mount.IsInStable = false;
                
                MountHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);
            }
        }

        public void StableToEquip(int mountId)
        {
            if (_availablesMounts.Remove(mountId) &&
                MountManager.Instance.GetMountById(mountId) is { } mount)
            {
                if (Character.EquipedMount is not null)
                    EquipToInventory(Character.EquipedMount.Id);

                if (Character.TryEquipMount(mount))
                {
                    mount.PaddockMapId = null;
                    mount.IsInStable = false;
                
                    MountHandler.SendExchangeMountStableRemoveMessage(Character.Client, mount);
                }
            }
        }
    }
}
