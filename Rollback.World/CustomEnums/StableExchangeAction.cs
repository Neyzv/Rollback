namespace Rollback.World.CustomEnums
{
    public enum StableExchangeAction : sbyte
    {
        EquipToStable = 1,
        StableToEquip = 2,
        StableToInventory = 4,
        InventoryToStable = 5,
        StableToPaddock = 6,
        PaddockToStable = 7,
        EquipToPaddock = 9,
        PaddockToEquip = 10,
        EquipToInventory = 13,
        PaddockToInventory = 14,
        InventoryToEquip = 15,
        InventoryToPaddock = 16
    }
}
