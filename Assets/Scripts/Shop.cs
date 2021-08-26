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
    Canvas interactionCanvas;

    public GameObject ShopCanvas { get { return shopCanvas; } private set { shopCanvas = value; } }     //Turn on and off the Shopping canvas

    [SerializeField] Image chestTest;
    [SerializeField] Image helmetTest;
    IShopBuyer buyer;

    float itemCost;
    public GameObject lastItemSelected { get; set; }

    Equipment myEquipment;

    public Action<Equipment, Color> OnBoughtItem;

    Stack<GameObject> cartStack = new Stack<GameObject>();
    [SerializeField] List<Image> emptySpaceImages = new List<Image>();
    [SerializeField] GameObject availableCoins = null;

    Dictionary<GameObject, int> armorDupplicates = new Dictionary<GameObject, int>();



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


    /// <summary>
    /// Add the last item selected to the cart and shows it on the side
    /// </summary>
    public void AddToCart()
    {
        if (lastItemSelected != null && !cartStack.Contains(lastItemSelected))  //Add a new Item in the cart
        {
            cartStack.Push(lastItemSelected);
            armorDupplicates.Add(lastItemSelected, 1);  // initialize the amount to 1
            Debug.Log($"{lastItemSelected.name} was added to the cart");
            GameObject spot = GetEmptySpot();
            spot.GetComponent<Image>().sprite = lastItemSelected.GetComponent<Image>().sprite;
            spot.GetComponent<Image>().color = lastItemSelected.GetComponent<Image>().color;
            itemCost += lastItemSelected.GetComponent<SelectedItem>().myPrice;
        }
        else if (lastItemSelected != null && cartStack.Contains(lastItemSelected))  //Add a multiple of one item
        {
            cartStack.Push(lastItemSelected);
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

        }
    }


    /// <summary>
    /// Remove one item selected from the cart
    /// </summary>
    /// <param name="index">index of a selected item</param>
    public void RemoveOneFromCart(int index)
    {
        Image[] children = emptySpaceImages[index-1].GetComponentsInChildren<Image>();
        foreach (var child in children)
        {
            if (child.GetComponentInChildren<Text>())
            {
                int previousAmount = int.Parse(child.GetComponentInChildren<Text>().text);
                previousAmount--;

                //If we removed all of the dupplicates
                if(previousAmount == 0)
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
                    break;
                }
                child.GetComponentInChildren<Text>().text = previousAmount.ToString();
                break;
            }
        }
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
