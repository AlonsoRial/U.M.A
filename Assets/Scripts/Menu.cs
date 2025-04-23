using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private Canvas[] canvas;
    [SerializeField] private GameObject jugador;

    public void Renudar()
    {
        canvas[0].enabled = false;
        Time.timeScale = 1.0f;
        jugador.SetActive(true);
    }

    

    public void IrOpciones() 
    {
        canvas[0].enabled = false ;
        canvas[1].enabled = true;
    }

    public void IrControles() 
    {
        canvas[1].enabled = false;
        canvas[2].enabled = true;
    }

    public void IrSonidos() 
    {
        canvas[1].enabled = false;
        canvas[3].enabled = true;
    }

    public void VolverAOpciones() 
    {
        foreach (Canvas canvaIndi in canvas)
        {
            canvaIndi.enabled = false;
        }

        canvas[1].enabled = true;
    }

    public void VolverAPausa() 
    {
        foreach (Canvas canvaIndi in canvas)
        {
            canvaIndi.enabled = false;
        }

        canvas[0].enabled = true;
    }



    public void Salir() 
    {
        Application.Quit();
    }

    public void Reiniciar() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

}
