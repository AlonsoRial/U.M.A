using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaEnMovimiento : MonoBehaviour
{

    [SerializeField] private Transform[] points;
    [SerializeField] private float velocidad;
    [SerializeField] private bool retorno;
    private int siguientePlataforma = 1;
    private bool ordenPlataforma = true;

   

    // Update is called once per frame
    void Update()
    {

        if (ordenPlataforma && siguientePlataforma + 1 >= points.Length) 
        {
            ordenPlataforma = false;
        }

        if (!ordenPlataforma && siguientePlataforma <= 0) 
        {
            ordenPlataforma = true;
        }

        if (Vector2.Distance(transform.position, points[siguientePlataforma].position)<0.1f) 
        {

            if (ordenPlataforma)
            {
                siguientePlataforma++;
            }
            else 
            {
                if (retorno)
                {
                    siguientePlataforma = 0;
                }
                else 
                {
                    siguientePlataforma--;
                }


            }

            

        }

        transform.position = Vector2.MoveTowards(transform.position, points[siguientePlataforma].position, velocidad * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public IEnumerator Cambio() 
    {
        yield return new WaitForSeconds(5); ;
    }

}
