using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IShopBuyer
{
    int coins;


    public int GetMyCoins()
    {
        return coins;
    }

    public void SpendCoins(int coin)
    {
        coins -= coin;
    }

    // Start is called before the first frame update
    void Start()
    {
        coins = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
