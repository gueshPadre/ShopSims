using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    [SerializeField] GameObject shopKeeper = null;      // Reference to the NPC shopkeeper
    [SerializeField] GameObject shopCanvas = null;      // Reference to ShopCanvas
    [SerializeField] GameObject sellCanvas = null;      // Reference to SellCanvas
    Canvas interactionCanvas;

    public GameObject ShopCanvas { get { return shopCanvas; } private set { shopCanvas = value; } }     //Turn on and off the Shopping canvas

    [SerializeField] Image chestTest;
    [SerializeField] Image helmetTest;
    IShopBuyer buyer;

    int itemCost;
    public GameObject lastItemSelected { get; set; }
    public GameObject lastItemSelectedToSell { get; set; }

    Equipment myEquipment;

    public Action<Equipment, Color> OnBoughtItem;

    List<GameObject> cartList = new List<GameObject>();
    List<GameObject> ownedList = new List<GameObject>();
    [SerializeField] List<Image> emptySpaceImages = new List<Image>();
    [SerializeField] GameObject availableCoins = null;
    [SerializeField] GameObject itCostsText = null;
    [SerializeField] GameObject warningCanvas = null;

    [SerializeField] List<Image> emptySellImages = new List<Image>();


    Dictionary<GameObject, int> armorDupplicates = new Dictionary<GameObject, int>();

    int lastAmountToGive;



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

            // Quick check for plural spelling if more than one coin
            if (buyer.GetMyCoins() > 1)
            {
                availableCoins.GetComponent<TMP_Text>().text = $"You have {buyer.GetMyCoins()} coins available";
            }
            else
            {
                availableCoins.GetComponent<TMP_Text>().text = $"You have {buyer.GetMyCoins()} coin available";
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        interactionCanvas.gameObject.SetActive(false);
        shopCanvas.SetActive(false);
        shopKeeper.GetComponentInChildren<Animator>().SetBool("isInShop", false);
        ResetStore();
    }

    #region choosing Colors

    /// <summary>
    /// Called when the player clicks on one shop item
    /// </summary>
    public void ChooseChestColorWhite()
    {
        chestTest.color = Color.white;
        myEquipment = Equipment.Chest;
    }

    public void ChooseChestColorRed()
    {
        chestTest.color = Color.red;
        myEquipment = Equipment.Chest;
    }

    public void ChooseChestColorGreen()
    {
        chestTest.color = Color.green;
        myEquipment = Equipment.Chest;
    }

    public void ChooseChestColorTeal()
    {
        chestTest.color = Color.cyan;
        myEquipment = Equipment.Chest;
    }

    public void ChooseHelmetColorWhite()
    {
        helmetTest.color = Color.white;
        myEquipment = Equipment.Helmet;
    }

    public void ChooseHelmetColorRed()
    {
        helmetTest.color = Color.red;
        myEquipment = Equipment.Helmet;
    }

    public void ChooseHelmetColorGreen()
    {
        helmetTest.color = Color.green;
        myEquipment = Equipment.Helmet;
    }

    public void ChooseHelmetColorTeal()
    {
        helmetTest.color = Color.cyan;
        myEquipment = Equipment.Helmet;
    }

    #endregion


    public void UpdateAvailableCoin()
    {
        availableCoins.GetComponent<TMP_Text>().text = $"You have {buyer.GetMyCoins()} coins available";
    }



    /// <summary>
    /// Add the last item selected to the cart and shows it on the side
    /// </summary>
    public void AddToCart()
    {
        if (lastItemSelected != null && !cartList.Contains(lastItemSelected))  //Add a new Item in the cart
        {
            cartList.Add(lastItemSelected);
            armorDupplicates.Add(lastItemSelected, 1);  // initialize the amount to 1
            GameObject spot = GetEmptySpot();
            spot.GetComponent<Image>().sprite = lastItemSelected.GetComponent<Image>().sprite;
            spot.GetComponent<Image>().color = lastItemSelected.GetComponent<Image>().color;

            itemCost += lastItemSelected.GetComponent<SelectedItem>().myPrice;
        }
        else if (lastItemSelected != null && cartList.Contains(lastItemSelected))  //Add a multiple of one item
        {
            cartList.Add(lastItemSelected);
            armorDupplicates[lastItemSelected]++;
            Image dupplicate = null;
            for (int i = 0; i < emptySpaceImages.Count; i++)
            {
                if (emptySpaceImages[i].sprite == lastItemSelected.GetComponent<Image>().sprite)
                {
                    dupplicate = emptySpaceImages[i];
                    break;
                }
            }

            // Turning the dupplicate sign on
            Image[] dupplicateSprites = dupplicate.GetComponentsInChildren<Image>();
            for (int i = 0; i < dupplicateSprites.Length; i++)
            {
                if (dupplicateSprites[i].GetComponentInChildren<Text>())     //if it has a text as children
                {
                    Color color = dupplicateSprites[i].GetComponentInChildren<Text>().color;
                    color.a = 100f;
                    dupplicateSprites[i].GetComponentInChildren<Text>().color = color;
                    dupplicateSprites[i].GetComponentInChildren<Text>().text = armorDupplicates[lastItemSelected].ToString();
                }
                Color imageColor = dupplicateSprites[i].color;
                imageColor.a = 100f;
                dupplicateSprites[i].color = imageColor;
            }
            itemCost += lastItemSelected.GetComponent<SelectedItem>().myPrice;
        }
        else    //Nothing was selected
        {
            GameObject sign = Instantiate(warningCanvas);
            Destroy(sign, 4f);  // destroy the sign after the animation 
        }
        UpdateCost();
    }

    void UpdateCost()
    {
        itCostsText.GetComponent<TMP_Text>().text = $"It costs {itemCost} coins";
        itCostsText.SetActive(true);
    }


    /// <summary>
    /// Remove one item selected from the cart
    /// </summary>
    /// <param name="index">index of a selected item</param>
    public void RemoveOneFromCart(int index)
    {
        if (emptySpaceImages[index - 1].GetComponent<Image>().color.a == 0) { return; }  // if not displayed, don't do anything
        Image[] children = emptySpaceImages[index - 1].GetComponentsInChildren<Image>();
        foreach (var child in children)
        {
            if (child.GetComponentInChildren<Text>())
            {
                int previousAmount = int.Parse(child.GetComponentInChildren<Text>().text);
                previousAmount--;

                //If we removed all of the dupplicates
                if (previousAmount == 0)
                {
                    Color old = child.GetComponentInChildren<Text>().color;
                    old.a = 0;
                    child.GetComponentInChildren<Text>().color = old;
                    emptySpaceImages[index - 1].GetComponent<Image>().sprite = null;
                    Color a = emptySpaceImages[index - 1].GetComponent<Image>().color;
                    a.a = 0;
                    emptySpaceImages[index - 1].GetComponent<Image>().color = a;
                    for (int i = 0; i < children.Length; i++)
                    {
                        Color c = children[i].color;
                        c.a = 0;
                        children[i].color = c;
                    }
                    lastItemSelected = null;
                    break;
                }
                child.GetComponentInChildren<Text>().text = previousAmount.ToString();
                break;
            }
        }
        if (lastItemSelected.GetComponent<SelectedItem>())
        {
            itemCost -= lastItemSelected.GetComponent<SelectedItem>().myPrice; 
        }
        UpdateCost();
    }

    /// <summary>
    /// Back from the store
    /// </summary>
    public void GoBack()
    {
        ResetStore();
        shopCanvas.SetActive(false);
    }


    /// <summary>
    /// Get the next empty image spot
    /// </summary>
    GameObject GetEmptySpot()
    {
        for (int i = 0; i < emptySpaceImages.Count; i++)
        {
            if (emptySpaceImages[i].sprite == null)
            {
                return emptySpaceImages[i].gameObject;
            }
        }
        return null;    // no empty space were found.
    }


    /// <summary>
    /// After purchasing an item, just reset the store
    /// </summary>
    void ResetStore()
    {
        for (int i = 0; i < emptySpaceImages.Count; i++)
        {
            emptySpaceImages[i].sprite = null;
            Color a = emptySpaceImages[i].color;
            a.a = 0f;
            emptySpaceImages[i].color = a;
        }
        itemCost = 0;
        itCostsText.SetActive(false);
    }


    public void BuyItem()
    {
        if (!cartList.Contains(lastItemSelected)) 
        {
            // show warning if nothing in Cart
            GameObject sign = Instantiate(warningCanvas);
            Destroy(sign, 4f);  // destroy the sign after the animation 
            return; 
        }   
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

            ownedList.Add(lastItemSelected);
            ResetStore();
            shopCanvas.SetActive(false);
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
        else
        {
            buyer.SpendCoins((int)itemCost);
            return true;
        }
    }

    public void GoToSellMenu()
    {
        //Display all the owned armor
        for (int i = 0; i < ownedList.Count; i++)
        {
            if(emptySellImages[i].sprite == null)   //not checking if i exists because it'll always be lower than the inventory we have
            {
                emptySellImages[i].sprite = ownedList[i].GetComponent<Image>().sprite;
                emptySellImages[i].color = ownedList[i].GetComponent<Image>().color;
            }
        }
        sellCanvas.SetActive(true);
        shopCanvas.SetActive(false);
    }

    public void BackToShop()
    {
        UpdateAvailableCoin();
        sellCanvas.SetActive(false);
        shopCanvas.SetActive(true);
    }

    public float CheckAmount(GameObject armor)
    {
        for (int i = 0; i < ownedList.Count; i++)
        {
            //Compares the color
            if(armor.GetComponent<Image>().color == ownedList[i].GetComponent<Image>().color)
            {
                float cashBack = ownedList[i].GetComponent<SelectedItem>().myPrice / 2;
                lastAmountToGive = (int)cashBack;
                return cashBack;
            }
        }
        return 0;
    }


    public void Sell()
    {
        for (int i = 0; i < ownedList.Count; i++)
        {
            if (ownedList[i].GetComponent<Image>().sprite == lastItemSelectedToSell.GetComponent<Image>().sprite)
            {
                lastItemSelectedToSell.GetComponent<Image>().sprite = null;
                Color temp = lastItemSelectedToSell.GetComponent<Image>().color;
                temp.a = 0;
                lastItemSelectedToSell.GetComponent<Image>().color = temp;
                buyer.ReceiveCoins(lastAmountToGive);
                lastAmountToGive = 0;   // reset the amount
                ownedList.RemoveAt(i);
                break;
            } 
        }
        BackToShop();
    }

}
