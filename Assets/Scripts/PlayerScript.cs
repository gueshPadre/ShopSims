using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IShopBuyer
{
    int coins;
    Shop shop;

    float timer;
    float timePeriod = 5f;


    public int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
            coinCanvas.GetComponentInChildren<TMP_Text>().text = $"Coins: {coins}";
        }
    }

    [SerializeField] GameObject coinCanvas = null;


    public int GetMyCoins()
    {
        return Coins;
    }

    public void ReceiveCoins(float coin)
    {
        Coins += (int)coin;
    }

    public void RegisterTheShop(Shop myShop)
    {
        shop = myShop;
    }

    public void SpendCoins(int coin)
    {
        Coins -= coin;
        shop.UpdateAvailableCoin();
    }

    // Start is called before the first frame update
    void Start()
    {
        Coins = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && shop != null)
        {
            shop.ShopCanvas.SetActive(true);
        }

        timer += Time.deltaTime;
        if(timer >= timePeriod)
        {
            Coins += 50;
            timer = 0f;     // Reset the timer
        }
    }



}
