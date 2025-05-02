using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaCarga : MonoBehaviour
{

    [SerializeField] private SceneAsset escena;
    [SerializeField] private int tiempo;

    // Start is called before the first frame update
    void Start() 
    {
        StartCoroutine(TiempodeCarga());
    }

    IEnumerator TiempodeCarga() 
    {
        yield return new WaitForSeconds(tiempo);
        SceneManager.LoadScene(escena.name);
    }

    
}
