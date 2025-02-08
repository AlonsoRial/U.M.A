using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodigoPuerta : MonoBehaviour
{

    public PlayerController jugador;

    BoxCollider2D boxCollider;
  //  SpriteRenderer spriteRenderer;

    bool estado;
    //hola f
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();   
    //    spriteRenderer = GetComponent<SpriteRenderer>();
     
    }

    // Update is called once per frame
    void Update()
    {
     //   var colorOriginal = spriteRenderer.color;

        estado = jugador.estadoSwitch;

        if (this.CompareTag("ROJO"))
        {
            estado = !estado;
        }



        boxCollider.enabled = estado;

        /*
                if (jugador.estadoSwitch)
                {
                    spriteRenderer.color = colorOriginal;


                }
                else 
                {
                    spriteRenderer.color = new Color(0, 0, 255, 255);

                }
        */
    }

    
}
