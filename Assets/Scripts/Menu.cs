using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update

    public InputActionAsset actions;

    
    //public Canvas panelControles;
    //public Canvas panelSonidos;
    //public Canvas MainCanva;

    public Canvas[] canvas;
    public GameObject jugador;

    public void Renudar()
    {
        canvas[0].enabled = false;
        Time.timeScale = 1.0f;
        jugador.SetActive(true);
    }

    public void Sonidos() 
    {
        canvas[2].enabled = true;
        canvas[0].enabled = false;
    }


    public void ActivaControles() 
    {
        canvas[1].enabled = true;
        canvas[0].enabled = false;
    }

    public void VolverAlMenu() 
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


    public void Guardar() 
    {
        PlayerPrefs.SetFloat("PosicionX" , jugador.transform.position.x);
        PlayerPrefs.SetFloat("PosicionY", jugador.transform.position.y);
        PlayerPrefs.SetFloat("PosicionZ", jugador.transform.position.z);
        PlayerPrefs.SetInt("nivel", SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.Save();
    }

    public void Cargar() 
    {
        jugador.transform.position = new Vector3(
            PlayerPrefs.GetFloat("PosicionX"), 
            PlayerPrefs.GetFloat("PosicionY"),
            PlayerPrefs.GetFloat("PosicionZ"));

        //SceneManager.LoadScene(PlayerPrefs.GetInt("nivel"));
        //Time.timeScale = 1.0f;

    }


}
