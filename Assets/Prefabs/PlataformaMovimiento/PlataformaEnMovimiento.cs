using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaEnMovimiento : MonoBehaviour
{

    [SerializeField] private Transform[] points;
    [SerializeField] private float velocidad;
    private float velocidadInicial;
    [SerializeField] private bool retorno;
    private int siguientePlataforma = 1;
    private bool ordenPlataforma = true;
    [SerializeField] private int tiempo = 3;


    private void Awake()
    {
        velocidadInicial = velocidad;
    }

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

            StartCoroutine(Cambio());

        }

        transform.position = Vector2.MoveTowards(transform.position, points[siguientePlataforma].position, velocidad * Time.deltaTime);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

    public IEnumerator Cambio() 
    {
        velocidad = 0;
        Debug.Log("HOLA");
        yield return new WaitForSeconds(tiempo);
        Debug.Log("Adios");
        velocidad = velocidadInicial;

    }
    //hola

}
