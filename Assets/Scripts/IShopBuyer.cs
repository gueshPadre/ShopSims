public interface IShopBuyer
{
    int GetMyCoins();

    void SpendCoins(int coin);

    void RegisterTheShop(Shop myShop);

    void ReceiveCoins(float coins);

}
