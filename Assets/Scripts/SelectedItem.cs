using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedItem : MonoBehaviour, IPointerDownHandler
{
    public int myPrice;


    public void OnPointerDown(PointerEventData eventData)
    {
        FindObjectOfType<Shop>().lastItemSelected = eventData.pointerCurrentRaycast.gameObject;
    }

    private void Start()
    {
        GetComponentInChildren<TMP_Text>().text = myPrice.ToString() + " coins";
    }

}
