using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ItemsToSell : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        Shop shop = FindObjectOfType<Shop>();
        shop.lastItemSelectedToSell = eventData.pointerCurrentRaycast.gameObject;

        GameObject.Find("AmountText").GetComponent<Text>().text = $"Amount: {shop.CheckAmount(shop.lastItemSelectedToSell)}";
    }
}
