using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaBehaviour : MonoBehaviour
{
    public GameObject [] posiciones; // Array de los objetos "posiciones" (donde va a ir el objeto)
    int posicionActual = 0; // Para que vaya en orden
    public float velocidad = 0.1f; //Velocidad a la que va 
    private Vector3 direccion; // Dirección 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direccion = new Vector3(posiciones[posicionActual].transform.position.x, posiciones[posicionActual].transform.position.y,0); // Aqui definimos la dirección para saber el vector que darle 
        //                      Coge la posición del objeto del objeto"posición" a la que va a ir en x e y 
        if (Vector3.Distance(transform.position, posiciones[posicionActual].transform.position) < 0.3f) // En caso de que la distancia de la posición del objeto y el objeto posición objetivo sea menor que un número entonces cambia de objetivo
        {
            posicionActual++; //El siguiente objetivo es el siguiente de la lista
            if (posicionActual > posiciones.Length - 1)
                posicionActual = 0; //En caso de que recorra la array entera, empieza desde 0 haciendo un bucle
        }
        transform.position = Vector3.Lerp(transform.position, direccion, velocidad*Time.deltaTime); // Mueve a la plataforma 
        //                             posición inicial |  El vector donde ir | Velocidad a la que va 
    }
}
