using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    float hori;
    float vert;

    public float Horizontal
    {
        get
        {
            return hori;
        }
        set
        {
            hori = value;
        }
    }


    public float Vertical
    {
        get
        {

            return vert;
        }
        set
        {
            vert = value;
        }
    }
    

    void Update()
    {
        ReadInputs();
    }

    void ReadInputs()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        Vertical = Input.GetAxisRaw("Vertical");

        transform.Translate(hori * Time.deltaTime * speed, vert * Time.deltaTime * speed, 0);
    }
}
