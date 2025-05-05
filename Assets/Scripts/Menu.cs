using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    [SerializeField] private Canvas[] canvas;
    [SerializeField] private GameObject jugador;

    [Header("Temporizados")]
    public float tiempo;
     private static float segundos;
    private static float tiempoInicial;
    [SerializeField] private TextMeshProUGUI tiempoUI;

    private void Start()
    {
        Segundos = tiempo;
        tiempoInicial = tiempo;
    }

    public static float Segundos { get => segundos; set => segundos = value; }
    public static float TiempoInicial { get => tiempoInicial; set => tiempoInicial = value; }

    private void Update()
    {
        if (segundos >= 0)
        {
            segundos -= 1 * Time.deltaTime;

            tiempoUI.SetText(Math.Truncate(segundos).ToString());
        }
        
    }

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
        canvas[4].enabled = true;
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
