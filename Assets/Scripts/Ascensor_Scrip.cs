using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ascensor_Scrip : MonoBehaviour
{

    [SerializeField] private SceneAsset escena;
    
    
 //gfd

    public void MenuInicio() 
    {
        SceneManager.LoadScene(escena.name);
    }

    public void Salir()
    {
        Application.Quit();
    }

}
