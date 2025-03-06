using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderData;
using static UnityEngine.InputSystem.InputAction;



//Hola Eric, te comento el codigo para que lo entiendas un poco mejor :)
public class PlayerController : MonoBehaviour
{
    // El PlayerInput maneja el nuevo sistema de inputs y la camara, si queremos que el jugador pueda cambiar los controles, se debe de hacer con esto, en teoria...

    Rigidbody2D rigidbody2;
    Vector2 vector2 = Vector2.zero;
    PlayerInput playerInput;
    Vector3 posicionGuardado;



    [Header("MOVIMIENTO")]
    [SerializeField] private float gravedad;
    [SerializeField] private float velocidad;
    [SerializeField] private float gravedadCaida;
    [SerializeField] private float MAX_Velocidad_Caida;
    private bool girando = true;




    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false;



    [Header("SALTOS")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float fuerzaSaltoDoble;
    [SerializeField] private float fuerzaDetencionSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Vector2 dimensionControladorSalto;
    [SerializeField] private bool botonSaltoDoble;
    [SerializeField] private int cantidadSaltosDobles;
    [SerializeField] private int saltosDoblesRestantes;




    [Header("SALTO MURO")]
    [SerializeField] private Transform controaldorMuro;
    [SerializeField] private Vector2 dimensionControladorMuro;




    [Header("DASH")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private bool botonDash;
    [SerializeField] private int cantidadDash;
    [SerializeField] private int dashRestantes;
    [SerializeField] private float tiempoDash;



    [Header("PAUSAR JUEGO")]
    [SerializeField] private Canvas canvas;
    private bool botonPausa;

    public bool EstadoSwitch { get => estadoSwitch; }


    public void Flip()
    {
        if (girando && vector2.x < 0f || !girando && vector2.x>0f) 
        {
            girando = !girando;
            Vector3 escalaLocal = transform.localScale;
            escalaLocal.x *= -1f;
            transform.localScale = escalaLocal;
        }

        /*
        if (vector2.x > 0)
        {
            rigidbody2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (vector2.x < 0)
        {
            rigidbody2.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        */
    }


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
      
        

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log(rigidbody2.velocity.y + " ||||| " + rigidbody2.gravityScale);
        Flip();


        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);


        if (EstaEnTierra() || (!EstaEnTierra() && EstaEnMuro() && vector2.x != 0))
        {
            dashRestantes = cantidadDash;

        }


        if (EstaEnTierra()) 
        {
            rigidbody2.gravityScale = gravedad;
            botonSaltoDoble = false;
            saltosDoblesRestantes = cantidadSaltosDobles;
        }

        if (rigidbody2.velocity.y<0)
        {
            rigidbody2.gravityScale += Time.deltaTime * gravedadCaida;
          
        }

        if (rigidbody2.velocity.y < MAX_Velocidad_Caida) 
        {
          //  Debug.Log("Sobrepasado");
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, MAX_Velocidad_Caida);
        }

    }


    private void FixedUpdate()
    {



        rigidbody2.velocity = new Vector2(vector2.x * velocidad, rigidbody2.velocity.y);
       


        //DASH
        if (botonDash && dashRestantes > 0)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadDash, 0);

            StartCoroutine(Espera());

        }


    }

 



    public IEnumerator Espera()
    {

        yield return new WaitForSeconds(tiempoDash);
        botonDash = false;

    }


    public bool EstaEnTierra()
    {
        return Physics2D.OverlapBox(controladorSuelo.position, dimensionControladorSalto, 0, capaSuelo);
    }

    public bool EstaEnMuro()
    {
        return Physics2D.OverlapBox(controaldorMuro.position, dimensionControladorMuro, 0, capaSuelo);
    }


    public void Mover(CallbackContext callbackContext)
    {
        vector2.x = callbackContext.ReadValue<Vector2>().x;


    }


    public void CambioSwitch(CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            estadoSwitch = !estadoSwitch;
        }
    }

    //hola
    public void Jump(CallbackContext callbackContext)
    {

        if (callbackContext.started && EstaEnTierra()) 
        {
            Debug.Log("started");
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, fuerzaSalto);
            
        }

        if (callbackContext.canceled && rigidbody2.velocity.y > 0) 
        {
            Debug.Log("canceled");
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, rigidbody2.velocity.y/fuerzaDetencionSalto);
            

        }

        if (callbackContext.canceled) 
        {
            botonSaltoDoble = true; 
        }

        if (callbackContext.started && botonSaltoDoble && saltosDoblesRestantes!=0) 
        {
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, 0);
            Debug.Log("SaltoDoble");
            rigidbody2.AddForce(new Vector2(rigidbody2.velocity.x, fuerzaSaltoDoble), ForceMode2D.Impulse);
            botonSaltoDoble = false;
            saltosDoblesRestantes--;
        }

        //Tal vez, si al started le ponemos un bool de habilitación del segundo salto, este se pueda dar
        /*
        if (callbackContext.started && !EstaEnTierra() && rigidbody2.velocity.y > 0)
        {
        
            rigidbody2.AddForce(new Vector2(rigidbody2.velocity.x, fuerzaSaltoDoble), ForceMode2D.Impulse);
            //rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, fuerzaSaltoDoble);
        }
        */

    }


    public void Dash(CallbackContext callbackContext)
    {

        if (callbackContext.started)
        {
            botonDash = true;

        }

        if (callbackContext.canceled)
        {
            --dashRestantes;
            if (dashRestantes < 0)
            {
                dashRestantes = 0;
            }
        }

    }

    public void Pausa(CallbackContext callbackContext)
    {

        if (callbackContext.started)
        {
            botonPausa = !botonPausa;
        }

        if (botonPausa)
        {
            Time.timeScale = 0;
            canvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            canvas.enabled = false;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("HAZARDS"))
        {
            Debug.Log("muerte");
            rigidbody2.transform.position = posicionGuardado;
        }

        if (collision.gameObject.CompareTag("CHECKPOINT"))
        {
            Debug.Log("checkpoint guardado");

            posicionGuardado = collision.transform.position;

        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionControladorSalto);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(controaldorMuro.position, dimensionControladorMuro);

    }

}
