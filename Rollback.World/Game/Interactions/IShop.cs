namespace Rollback.World.Game.Interactions
{
    public interface IShop
    {
        void BuyItem(int id, int quantity);
        void SellItem(int id, int quantity);
    }
}
