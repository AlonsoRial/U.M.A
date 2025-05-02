using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ascensor_Scrip : MonoBehaviour
{

    [SerializeField] private SceneAsset escena;
    
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(escena.name);
            
        }
    }

}
