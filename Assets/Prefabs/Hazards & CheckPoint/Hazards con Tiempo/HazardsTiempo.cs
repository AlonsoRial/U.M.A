using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HazardPinchos : MonoBehaviour
{

    [SerializeField] private float tiempoDesactivado;
    [SerializeField] private float tiempoActivado;
    [SerializeField]  private float tiempoDurmiendo;
    private SpriteRenderer sprite;
    private BoxCollider2D boxcollider2D;
    private float tiempo;
  
    void Awake()
    {
       
        sprite = GetComponent<SpriteRenderer>();
        boxcollider2D = GetComponent<BoxCollider2D>();
       
        StartCoroutine(PinchosTime());
    }


    IEnumerator PinchosTime()
    {
        yield return new WaitForSeconds(tiempoDurmiendo);
       
        while (true)
        {

            tiempo = boxcollider2D.enabled == true ? tiempoActivado : tiempoDesactivado;
            
            yield return new WaitForSeconds(tiempo);
            sprite.enabled = !sprite.enabled;
            boxcollider2D.enabled = !boxcollider2D.enabled;
          
        }
    }

}
