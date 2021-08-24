using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    /// <summary>
    /// Triggers when a player can interact with a shop
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IShopBuyer buyer = collision.gameObject.GetComponent<IShopBuyer>();
        if (buyer != null)
        {
            Debug.Log($"You've just entered possibly the shop with {buyer.GetMyCoins()} coins"); 
        }
    }
}
