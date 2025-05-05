using UnityEngine;

public class Turreta : MonoBehaviour
{
    [Header("Configuración General")]
    [SerializeField] private float rangoDeteccion = 5f;
    [SerializeField] private float velocidadRotacion = 5f;
    [SerializeField] private bool puedeDisparar = true;
    [SerializeField] private LayerMask capaSuelo; // Referencia al layer "Suelo"

    [Header("Configuración de Disparo")]
    [SerializeField] private GameObject prefabBala;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float cadenciaDisparo = 2f;
    [SerializeField] private float fuerzaRetroceso = 1f;
    private float tiempoUltimoDisparo;
    private Rigidbody2D rb;

    [Header("Referencias")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Transform jugador;
    private bool jugadorEnRango;
    private bool lineaDeVisionLibre;

    private void Start()
    {
        GameObject objetoJugador = GameObject.FindGameObjectWithTag("Player");
        if (objetoJugador != null)
        {
            jugador = objetoJugador.transform;
        }
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (jugador == null) return;

        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);
        jugadorEnRango = distanciaAlJugador <= rangoDeteccion;

        if (jugadorEnRango)
        {
            Vector2 direccionAlJugador = (jugador.position - transform.position).normalized;
            float angulo = Mathf.Atan2(direccionAlJugador.y, direccionAlJugador.x) * Mathf.Rad2Deg + 90f;
            
            // Verificar si hay obstáculos
            lineaDeVisionLibre = !HayObstaculosHaciaJugador();

            Quaternion rotacionObjetivo = Quaternion.Euler(0, 0, angulo);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = Mathf.Abs(angulo - 90f) > 90f;
            }

            // Solo disparar si no hay obstáculos
            if (puedeDisparar && lineaDeVisionLibre)
            {
                Disparar();
            }
        }
    }

    private bool HayObstaculosHaciaJugador()
    {
        Vector2 direccionAlJugador = jugador.position - transform.position;
        float distancia = direccionAlJugador.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionAlJugador.normalized, distancia, capaSuelo);
        
        // Dibujar el rayo en el editor para debugging
        Debug.DrawRay(transform.position, direccionAlJugador, hit ? Color.red : Color.green);
        
        return hit.collider != null;
    }

    private void Disparar()
    {
        if (Time.time > tiempoUltimoDisparo + cadenciaDisparo)
        {
            if (puntoDisparo != null && prefabBala != null)
            {
                Quaternion rotacionBala = transform.rotation * Quaternion.Euler(0, 0, 180f);
                GameObject bala = Instantiate(prefabBala, puntoDisparo.position, rotacionBala);
                
                if (rb != null)
                {
                    Vector2 direccionRetroceso = -transform.up;
                    rb.AddForce(direccionRetroceso * fuerzaRetroceso, ForceMode2D.Impulse);
                }
                
                tiempoUltimoDisparo = Time.time;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}