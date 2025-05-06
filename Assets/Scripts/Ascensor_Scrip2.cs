using UnityEngine;
using UnityEngine.SceneManagement;

public class Ascensor_Scrip2 : MonoBehaviour
{
    [SerializeField] private string nombreEscena;
    private Animator animator;

    private void Start()
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