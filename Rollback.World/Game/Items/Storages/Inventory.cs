using Rollback.Common.Logging;
using Rollback.Common.ORM;
using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects;
using Rollback.World.Game.Effects.Handlers.Items.Usable;
using Rollback.World.Game.Items.Types;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Game.World.Maps;
using Rollback.World.Handlers.Inventory;

namespace Rollback.World.Game.Items.Storages
{
    public sealed class Inventory : SaveableItemsStorage<Character, CharacterItemRecord, PlayerItem, Inventory>
    {
        public const int MaxKamasInInventory = 2_000_000_000;

        protected override int OwnerId =>
            Owner.Id;

        public override int MaxPods =>
            Owner.Stats[Stat.Weight].Total;

        public Inventory(Character owner) : base(owner)
        {
            ItemAdded += OnItemAdded;
            ItemRemoved += OnItemRemoved;
            ItemQuantityChanged += OnItemQuantityChanged;
        }

        #region Events
        public event Action<PlayerItem>? ItemUsed;
        #endregion

        private void OnItemAdded(PlayerItem item)
        {
            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);

            if (IsFull)
                Owner.Refresh();
        }

        private void OnItemRemoved(PlayerItem item, bool delete)
        {
            if (item.IsEquipped)
                MoveItem(item.UID, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

            InventoryHandler.SendObjectDeletedMessage(Owner.Client, item);

            if (!IsFull)
                Owner.Refresh();
        }

        private void OnItemQuantityChanged(PlayerItem item) =>
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);

        protected override void Load()
        {
            try
            {
                foreach (var item in DatabaseAccessor.Instance.Select<CharacterItemRecord>(string.Format(CharacterItemRelator.GetByOwnerId, Owner.Id)))
                {
                    if (item.Template is null)
                        throw new Exception($"Can not find template {item.ItemId} for item {item.UID}");

                    var playerItem = ItemManager.Instance.CreatePlayerItem(item, this);

                    if (TryStack(playerItem) is null)
                        _items.TryAdd(item.UID, playerItem);

                    if (playerItem.IsEquipped)
                    {
                        HandleItemBoost(playerItem, true);
                        playerItem.UpdateItemSkin(Owner);

                        playerItem.OnEquipped();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Instance.LogWarn($"Force disconnection of client {Owner.Client} while loading inventory : {e.Message}");
                Owner.Client.Dispose();
            }
        }

        public static bool IsCorrectPosition(ItemType type, CharacterInventoryPositionEnum position) =>
            type switch
            {
                ItemType.Chapeau => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT,
                ItemType.Cape or ItemType.SacADos => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE,
                ItemType.Bottes => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS,
                ItemType.Anneau => position is CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT or CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT,
                ItemType.Amulette => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET,
                ItemType.Ceinture => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT,
                ItemType.Bouclier => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD,

                ItemType.Dofus => position is CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1 or
                                  CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2 or
                                  CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3 or
                                  CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4 or
                                  CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5 or
                                  CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6,

                ItemType.Arc or ItemType.Dague or ItemType.Marteau or ItemType.Baguette or
                ItemType.Baton or ItemType.Epee or ItemType.Hache or ItemType.Faux or ItemType.FiletDeCapture =>
                    position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON,

                ItemType.Familier => position is CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS,

                _ => false,
            };

        public PlayerItem? GetEquipedItem(CharacterInventoryPositionEnum position) =>
            _items.Values.FirstOrDefault(x => x.Position == position);

        public int GetAmountOfEquipedItemsForSet(short setId) =>
            _items.Count(x => x.Value.IsEquipped && x.Value.ItemSetId == setId);

        public PlayerItem? AddItem(short itemId, int quantity = 1, EffectGenerationType generationType = EffectGenerationType.Normal,
            IEnumerable<EffectBase>? effects = null, bool send = true)
        {
            var result = default(PlayerItem);
            var template = ItemManager.Instance.GetTemplateRecordById(itemId);

            if (template is not null)
            {
                result = AddItem(ItemManager.Instance.CreatePlayerItem(this, template, quantity, generationType,
                    effects));

                if (send)
                    //Tu as obtenu %1 \'$item%2\'.
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, quantity, itemId);
            }

            return result;
        }

        private void HandleItemBoost(PlayerItem item, bool apply)
        {
            foreach (var effect in item.Effects)
                EffectManager.Instance.HandleItemEffect(Owner, effect, apply);

            if (item.SetEffects is not null)
            {
                var nbrOfItems = GetAmountOfEquipedItemsForSet(item.ItemSetId);

                if (apply ? nbrOfItems > 2 : nbrOfItems > 0) // Unapply last set bonus
                {
                    foreach (var effect in item.SetEffects.ElementAt(apply ? nbrOfItems - 3 : nbrOfItems - 1))
                        EffectManager.Instance.HandleItemEffect(Owner, effect, false);
                }

                if (nbrOfItems > 1)
                {
                    foreach (var effect in item.SetEffects.ElementAt(nbrOfItems - 2))
                        EffectManager.Instance.HandleItemEffect(Owner, effect, true);
                }
            }
        }

        public void MoveItem(PlayerItem item, CharacterInventoryPositionEnum position)
        {
            if (item.Position != position && HaveItem(item))
            {
                if (position is not CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                {
                    var equipedItem = GetEquipedItem(position);

                    if (equipedItem is not null)
                    {
                        if (item.Drop(equipedItem) || equipedItem.Feed(item))
                        {
                            RemoveItem(item, item.Quantity);
                            return;
                        }
                    }

                    if (!IsCorrectPosition(item.TypeId, position))
                        return;

                    if (Owner.Level >= item.Level)
                    {
                        if (item is { TypeId: ItemType.Anneau, ItemSetId: > 0 } or { TypeId: ItemType.Dofus })
                        {
                            var doublon = _items.Values.FirstOrDefault(x => x.IsEquipped && x.Id == item.Id);
                            if (doublon is not null)
                            {
                                position = doublon.Position;
                                MoveItem(doublon, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                            }
                        }
                        else if (item.TypeId is ItemType.Bouclier)
                        {
                            var weapon = GetEquipedItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
                            if (weapon is not null && weapon.TwoHanded is true)
                            {
                                MoveItem(weapon, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                                //Vous avez dû lâcher votre arme à deux mains pour équiper un bouclier.
                                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 78);
                            }
                        }
                        else if (item is { IsWeapon: true, TwoHanded: true })
                        {
                            var shield = GetEquipedItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
                            if (shield is not null)
                            {
                                MoveItem(shield, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                                //Vous avez dû lâcher votre bouclier pour équiper une arme à deux mains.
                                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 79);
                            }
                        }

                        if (equipedItem is not null)
                            MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                        item = TryUnStack(item, 1);
                    }
                    else
                    {
                        InventoryHandler.SendObjectErrorMessage(Owner.Client, ObjectErrorEnum.LEVEL_TOO_LOW);
                        return;
                    }
                }

                item.Position = position;
                item.LastInteractionDate = DateTime.Now;

                InventoryHandler.SendObjectMovementMessage(Owner.Client, item);

                if (position is CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                    TryStack(item);

                var apply = position is not CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

                HandleItemBoost(item, apply);

                CheckEquipedItemsCriterions();
                item.UpdateItemSkin(Owner);

                Owner.RefreshStats();
                Refresh();

                if (apply)
                    item.OnEquipped();
                else
                    item.OnUnequipped();
            }
        }

        public void MoveItem(int uid, CharacterInventoryPositionEnum position)
        {
            if (_items.ContainsKey(uid))
                MoveItem(_items[uid], position);
        }

        public void CheckEquipedItemsCriterions()
        {
            foreach (var item in _items.Values.Where(x => x.IsEquipped && x.Criterion is not null))
            {
                if (item.Criterion?.Eval(Owner) == false)
                {
                    MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    //Tes caractéristiques ne conviennent pas
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                }
            }
        }

        public void DropItem(int uid, int quantity)
        {
            if (quantity > 0)
            {
                if (_items.TryGetValue(uid, out var item) && item.Quantity >= quantity)
                {
                    item = TryUnStack(item, quantity);
                    InventoryHandler.SendObjectDeletedMessage(Owner.Client, item);
                    _items.Remove(item.UID, out _);

                    var cell = Owner.Cell.Point.GetAdjacentCells(x => Owner.MapInstance.IsCellFree(x) && !Owner.MapInstance.IsItemOnCell(x)).FirstOrDefault();

                    if (cell is not null && Owner.MapInstance.GetItems().Length < MapInstance.MaxItemByMap)
                    {
                        Owner.MapInstance.AddItem(item, cell.CellId);

                        Owner.MapInstance.TriggerCell(Owner, cell.CellId, World.Maps.Triggers.CellTriggerType.DropItem);
                    }
                    else
                        //Il n'y a pas assez de place ici.
                        Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 145);
                }
                else
                    //Vous ne possédez pas l'objet en quantité suffisante.
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 4);
            }
        }

        public void UseItem(int uid, int amount = 1, Cell? targetedCell = default)
        {
            var item = GetItemByUID(uid);
            if (item is not null && item.Quantity >= amount && (item.Criterion is null || item.Criterion.Eval(Owner)))
            {
                var handlers = item.Effects.Select(x => EffectManager.Instance.HandleUsableItemEffect(Owner, x,
                            targetedCell is null || !item.Targetable ? Owner.Cell : targetedCell)).OfType<UsableEffectHandler>().ToArray();

                if (handlers.Length is not 0 || item.Use(targetedCell))
                {
                    var amountToDelete = 1;

                    for (; amountToDelete < amount && item.Use(targetedCell); amountToDelete++)
                    {
                        foreach (var handler in handlers)
                            handler.Apply();

                        ItemUsed?.Invoke(item);
                    }

                    if (amountToDelete > 0)
                        RemoveItem(item, amountToDelete);
                }
            }
            else
                // Certaines conditions ne sont pas satisfaites
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
        }

        public void RefreshItem(int uid)
        {
            if (GetItemByUID(uid) is { } item)
                InventoryHandler.SendObjectModifiedMessage(Owner.Client, item);
        }

        public override void Refresh()
        {
            InventoryHandler.SendKamasUpdateMessage(Owner.Client);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);
        }
    }
}
