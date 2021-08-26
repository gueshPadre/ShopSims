using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        IShopBuyer buyer = collision.GetComponent<IShopBuyer>();
        // if it's a player/buyer
        if(buyer != null)
        {
            float randomCoinNb = Random.Range(0, 50);

            buyer.ReceiveCoins(randomCoinNb);

            this.gameObject.SetActive(false);
        }
    }
}
