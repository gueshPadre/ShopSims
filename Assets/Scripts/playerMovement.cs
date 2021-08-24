using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    float animSpeed;    //The float condition of the animator for the horizontal movement
    float animYSpeed;   //The float condition of the animator for the vertical movement

    float hori;
    float vert;

    /// <summary>
    /// Change the animator speed when the horizontal movement changes
    /// </summary>
    public float Horizontal
    {
        get
        {
            return hori;
        }
        set
        {
            animSpeed = value;
            hori = value;
        }
    }


    /// <summary>
    /// Change the animator speed when the vertical movement changes
    /// </summary>
    public float Vertical
    {
        get
        {

            return vert;
        }
        set
        {
            animYSpeed = value;
            vert = value;
        }
    }

    
    Animator myAnimator;

    void Start()
    {
        //Hold ref to the player animator
        myAnimator = GetComponentInChildren<Animator>();
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

        myAnimator.SetFloat("speed", animSpeed);
        myAnimator.SetFloat("YSpeed", animYSpeed);

    }
}
