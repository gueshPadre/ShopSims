using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinHandler : MonoBehaviour
{
    //[SerializeField] List<GameObject> skinGo = null;

    public static SkinHandler Instance;
    [SerializeField] GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        FindObjectOfType<Shop>().OnBoughtItem += ChangeColor;
    }


    public void ChangeColor(Shop.Equipment equip, Color color)
    {
        if (equip == Shop.Equipment.Chest)
        {
            //for (int i = 0; i < skinGo.Count; i++)
            //{
            SpriteRenderer[] layers = player.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < layers.Length; j++)
                {
                    if (j == 1 /*&& j == layers[j].sortingOrder + 1*/)     //1 is the index of the chest
                    {
                        layers[j].GetComponent<SpriteRenderer>().color = color;
                        //layers[j].GetComponent<SpriteRenderer>().material = null;
                    }
                }
            //}
        }
        else if (equip == Shop.Equipment.Helmet)
        {
            //for (int i = 0; i < skinGo.Count; i++)
            //{
                SpriteRenderer[] layers = player.GetComponentsInChildren<SpriteRenderer>();
                for (int j = 0; j < layers.Length; j++)
                {
                    if (j == 2 /*&& j == layers[j].sortingOrder + 1*/)     //1 is the index of the chest
                    {
                        //GameObject newOne = Instantiate(skinGo[i]);
                        //layers[j].GetComponent<SpriteRenderer>().material = null;
                        layers[j].GetComponent<SpriteRenderer>().color = color;
                    }
                }
            //}
        }
    }

    private void OnDestroy()
    {
        if (FindObjectOfType<Shop>())
        {
            FindObjectOfType<Shop>().OnBoughtItem -= ChangeColor; 
        }
    }

}
