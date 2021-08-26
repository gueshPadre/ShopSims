using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IShopBuyer
{
    int coins;
    Shop shop;



    public int GetMyCoins()
    {
        return coins;
    }

    public void RegisterTheShop(Shop myShop)
    {
        shop = myShop;
    }

    public void SpendCoins(int coin)
    {
        coins -= coin;
        shop.UpdateAvailableCoin();
    }

    // Start is called before the first frame update
    void Start()
    {
        coins = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J) && shop != null)
        {
            shop.ShopCanvas.SetActive(true);
        }
    }



}
