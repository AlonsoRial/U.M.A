using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Configuración de la Bala")]
    [SerializeField] private float fuerzaDisparo = 20f;
    [SerializeField] private float tiempoVida = 3f;
    
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * fuerzaDisparo, ForceMode2D.Impulse);
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("PARKOUR"))
        {
            Destroy(gameObject);
        }
    }
}