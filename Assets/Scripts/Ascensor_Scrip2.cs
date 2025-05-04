using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ascensor_Scrip2 : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;
    [SerializeField] private SceneAsset escena;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.SetActive(false);
            animator.SetTrigger("Funciona");
           
        }
    }

    void Cambio() 
    {
  
        SceneManager.LoadScene(escena.name);
    }

}
