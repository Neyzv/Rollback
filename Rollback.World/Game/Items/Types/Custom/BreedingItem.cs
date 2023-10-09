using Rollback.World.CustomEnums;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Effects.Types;
using Rollback.World.Game.Items.Attributes;
using Rollback.World.Game.Items.Storages;
using Rollback.World.Game.Mounts;
using Rollback.World.Game.World.Maps;

namespace Rollback.World.Game.Items.Types.Custom;

[ItemType((short)ItemType.ObjetDÉlevage)]
public sealed class BreedingItem : PlayerItem
{
    private EffectDice? _durabilityEffect;

    public EffectDice? DurabilityEffect =>
        _durabilityEffect ??= Effects.OfType<EffectDice>()
            .FirstOrDefault(x => x.Id is EffectId.EffectBreedingItemDurability);
    
    public BreedingItem(CharacterItemRecord record, Inventory inventory)
        : base(record, inventory) =>
        Created += OnBreedingItemCreated;

    private void OnBreedingItemCreated(PlayerItem obj)
    {
        Effects.Clear();

        if (ItemManager.Instance.GetBreedingItemDurability(Id) is { } durability)
        {
            _durabilityEffect = new EffectDice()
            {
                Id = EffectId.EffectBreedingItemDurability,
                DiceFace = durability,
                Value = durability
            };
            
            Effects.Add(_durabilityEffect);
        }   
    }

    public override bool Use(Cell? targetedCell) =>
        targetedCell is not null && MountManager.Instance.GetPaddock(_storage.Owner.MapInstance.Map.Record.Id)?
            .AddItem(_storage.Owner, this, targetedCell) == true;
}