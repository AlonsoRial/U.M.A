using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Ascensor_Scrip : MonoBehaviour
{
    [SerializeField] private Object escena;

    public void MenuInicio() 
    {
        if (escena == null)
        {
            Debug.LogError("Por favor, arrastra una escena al campo en el Inspector");
            return;
        }
        
        SceneManager.LoadScene(escena.name);
    }
    
    public void Salir()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    public void Jugar() 
    {
        SceneManager.LoadScene("Part1");
    }

}