using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    [SerializeField] GameObject shopKeeper = null;      // Reference to the NPC shopkeeper
    [SerializeField] GameObject shopCanvas = null;      // Reference to ShopCanvas
    Canvas interactionCanvas;

    public GameObject ShopCanvas { get { return shopCanvas; } private set { shopCanvas = value; } }     //Turn on and off the Shopping canvas

    [SerializeField] Image chestTest;
    [SerializeField] Image helmetTest;
    IShopBuyer buyer;

    float itemCost;

    Equipment myEquipment;

    public Action<Equipment, Color> OnBoughtItem;


    public enum Equipment
    {
        Chest,
        Helmet
    }


    private void Start()
    {
        interactionCanvas = shopKeeper.GetComponentInChildren<Canvas>();
        interactionCanvas.gameObject.SetActive(false);      // Set to false when starting game
    }

    /// <summary>
    /// Triggers when a player can interact with a shop
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        buyer = collision.gameObject.GetComponent<IShopBuyer>();
        if (buyer != null)
        {
            buyer.RegisterTheShop(this);
            shopKeeper.GetComponentInChildren<Animator>().SetBool("isInShop", true);
            interactionCanvas.gameObject.SetActive(true);
            Debug.Log($"You've just entered possibly the shop with {buyer.GetMyCoins()} coins");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionCanvas.gameObject.SetActive(false);
        shopCanvas.SetActive(false);
        shopKeeper.GetComponentInChildren<Animator>().SetBool("isInShop", false);
    }

    #region choosing Colors

    /// <summary>
    /// Called when the player clicks on one shop item
    /// </summary>
    public void ChooseChestColorWhite()
    {
        chestTest.color = Color.white;
        itemCost = 30f;
        myEquipment = Equipment.Chest;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Chest, Color.white);
    }

    public void ChooseChestColorRed()
    {
        chestTest.color = Color.red;
        itemCost = 50f;
        myEquipment = Equipment.Chest;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Chest, Color.red);
    }

    public void ChooseChestColorGreen()
    {
        chestTest.color = Color.green;
        itemCost = 60f;
        myEquipment = Equipment.Chest;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Chest, Color.green);
    }

    public void ChooseChestColorTeal()
    {
        chestTest.color = Color.cyan;
        itemCost = 100f;
        myEquipment = Equipment.Chest;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Chest, Color.cyan);
    }

    public void ChooseHelmetColorWhite()
    {
        helmetTest.color = Color.white;
        itemCost = 45f;
        myEquipment = Equipment.Helmet;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Helmet, Color.white);
    }

    public void ChooseHelmetColorRed()
    {
        helmetTest.color = Color.red;
        itemCost = 65f;
        myEquipment = Equipment.Helmet;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Helmet, Color.red);
    }

    public void ChooseHelmetColorGreen()
    {
        helmetTest.color = Color.green;
        itemCost = 75f;
        myEquipment = Equipment.Helmet;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Helmet, Color.green);
    }

    public void ChooseHelmetColorTeal()
    {
        helmetTest.color = Color.cyan;
        itemCost = 125f;
        myEquipment = Equipment.Helmet;
        //SkinHandler.Instance.ChangeColor(Shop.Equipment.Helmet, Color.cyan);
    }

    #endregion

    public void BuyItem()
    {
        if (CanBuy())
        {
            if (myEquipment == Equipment.Chest)
            {
                OnBoughtItem?.Invoke(myEquipment, chestTest.color);
            }
            else
            {
                OnBoughtItem?.Invoke(myEquipment, helmetTest.color);
            }
        }
        else
        {
            Debug.Log($"You're missing coins because it costs {itemCost} and you have {buyer.GetMyCoins()}");
        }
    }

    bool CanBuy()
    {
        if (buyer.GetMyCoins() < itemCost)
        {
            return false;
        }
        else return true;
    }


}
