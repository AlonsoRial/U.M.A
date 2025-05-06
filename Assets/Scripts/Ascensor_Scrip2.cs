using UnityEngine;
using UnityEngine.SceneManagement;

public class Ascensor_Scrip2 : MonoBehaviour
{
    [SerializeField] private string nombreEscena;
    
    void Cambio() 
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogError("Nombre de escena no especificado");
        }
    }
}