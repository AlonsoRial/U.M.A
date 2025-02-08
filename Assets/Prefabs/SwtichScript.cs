using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwtichScript : MonoBehaviour
{

    public PlayerController jugador;

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    public bool estado;
    //tonto quien lo lea XD
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();   
        spriteRenderer = GetComponent<SpriteRenderer>();
     
    }

    // Update is called once per frame
    void Update()
    {


        estado = jugador.EstadoSwitch;

        if (this.CompareTag("ROJO"))
        {
            estado = !estado;
        }

        boxCollider.enabled = estado;
        
        
        if (estado)
        {

            if (this.CompareTag("ROJO"))
            {
                spriteRenderer.color = new Color(255, 0, 0, 255);
            }

            if (this.CompareTag("VERDE"))
            {
                spriteRenderer.color = new Color(0, 255, 0, 255);
            }

        }
        else 
        {
            spriteRenderer.color = new Color(0, 0, 255, 255);

        }
       
    }

    
}
