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
    // ahh estamos comediantes
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();   //colision del muro
        spriteRenderer = GetComponent<SpriteRenderer>(); //cambio de los sprites del muro
     
    }

    // Update is called once per frame
    void Update()
    {

        //guarda en su variable el estado del switch del jugador
        estado = jugador.EstadoSwitch;

        if (this.CompareTag("SWITCH-ROJO")) //para que un muro esté activado y el otro no
        {
            estado = !estado;
        }

        boxCollider.enabled = estado; //habilita o desabilita la colisión del muro
        
        
        //cambia el color del muro, cuando tengamos sprites, en vez de cambiar de color, se cambiará los sprites
        // top 10 las peores mentiras del anime
        if (estado)
        {
            jugador.luces[0].enabled = true;
            jugador.luces[1].enabled = false;

            if (this.CompareTag("SWITCH-ROJO"))
            {
                //spriteRenderer.color = new Color(255, 255, 255, 255);
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
               
            

            }

            

            if (this.CompareTag("SWITCH-VERDE"))
            {
                //spriteRenderer.color = new Color(255, 255, 255, 255);
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
              
            }

        }
        else 
        {

            //spriteRenderer.color = new Color(0, 0, 255, 255);
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.1f);
            jugador.luces[0].enabled = false;
            jugador.luces[1].enabled = true;


        }
       
    }

    
}
