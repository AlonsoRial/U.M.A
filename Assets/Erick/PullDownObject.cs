using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PullDownObject : MonoBehaviour
{
    public float cooldown = 2f;              // Tiempo entre ca�das
    public float returnSpeed = 5f;           // Velocidad de regreso
    public bool isPulledUp = true;           // �Est� arriba?

    private Vector2 startPos;
    private Rigidbody2D rb;
    private bool isReturning = false;
    private float distanceTraveled = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        rb.gravityScale = 0f;  // Inicialmente sin gravedad
        StartCoroutine(StartCooldown());
    }

    void Update()
    {
        if (isReturning)
        {
            // Movimiento hacia arriba sin f�sica
            transform.position = Vector2.MoveTowards(transform.position, startPos, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, startPos) < 0.01f)
            {
                transform.position = startPos;
                isReturning = false;
                isPulledUp = true;
                StartCoroutine(StartCooldown());
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        // Iniciar ca�da
        isPulledUp = false;
        rb.gravityScale = 3f; // Activar gravedad
        distanceTraveled = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isPulledUp && collision.collider.CompareTag("Pullup"))
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
            distanceTraveled = startPos.y - transform.position.y;
            Debug.Log("Distancia de ca�da: " + distanceTraveled.ToString("F2") + " unidades");

            isReturning = true;
        }
    }
}
