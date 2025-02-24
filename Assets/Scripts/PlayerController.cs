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
    Vector3 posicionGuardado;

    PlayerInput playerInput;


    [Header("MOVIMIENTO")]
    [SerializeField] private float maxVelocidadMovimiento;
    [SerializeField] private float velocidad;
    [SerializeField] private float aceleracion;
    private Vector2 ultimoMovimiento;


    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false;


    [Header("SALTO")]
    [SerializeField] private float velocidadSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Vector2 dimensionControladorSalto;
    private bool botonSalto;
    [SerializeField] private int cantidadSaltos;
    [SerializeField] private int saltosRestantes;
    [SerializeField] private int cantidadSaltosDobles;
    [SerializeField] private int saltosRestantesDobles;
    private Vector2 vectorSaltoMovimiento;

    [Header("Regulaci�n Salto")]
    [Range(0, 1)][SerializeField] private float multiplicadorCancelarSalto;
    [SerializeField] private float multiplicadorGavedad;
    private float escalaGravedad;

    private bool botonSaltoArriba = true;



    [Header("SALTO MURO")]
    [SerializeField] private Transform controaldorMuro;
    [SerializeField] private Vector2 dimensionControladorMuro;

    [SerializeField] private float velocidadDeslizamiento;
    [SerializeField] private Vector2 fuerzaSaltoMuro;
    [SerializeField] private float tiempoSaltoMuro;
    [SerializeField] private bool saltoDeParez;



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


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        escalaGravedad = rigidbody2.gravityScale;
       
    }

    // Update is called once per frame
    void Update()
    {

        Flip();

      
        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);


        if (EstaEnTierra() || (!EstaEnTierra() && EstaEnMuro() && vector2.x!=0 )) {
            saltosRestantes = cantidadSaltos;
            dashRestantes = cantidadDash;
            saltosRestantesDobles = cantidadSaltosDobles;
        }

        if (vector2.x!=0) 
        {
            if (velocidad < maxVelocidadMovimiento)
            {
                velocidad += aceleracion * Time.deltaTime;
            }
        }
        else 
        {
            if (velocidad > 0)
            {
                velocidad -= 2* aceleracion * Time.deltaTime;
            }
        }


    }


    private void FixedUpdate()
    {
        //cacatua

        

        if (botonSalto && botonSaltoArriba ) 
        {


            if (saltosRestantes > 0 && EstaEnTierra())
            {
                Salto();
                --saltosRestantes;

            }
            else if (saltosRestantesDobles > 0 && !EstaEnTierra() ) 
            {
                Salto();
                --saltosRestantesDobles;
            }
           
   

        }

        if (rigidbody2.velocity.y < 0 && !EstaEnTierra())
        {
            rigidbody2.gravityScale = escalaGravedad * multiplicadorGavedad;
        }
        else 
        {
            rigidbody2.gravityScale = escalaGravedad;
        }


        
        if (!botonDash && !EstaEnMuro() && !saltoDeParez && vector2.x!=0)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidad, rigidbody2.velocity.y);
        }
        else if (!botonDash && !EstaEnMuro() && !saltoDeParez && vector2.x == 0) 
        {
            rigidbody2.velocity = new Vector2(ultimoMovimiento.x * velocidad, rigidbody2.velocity.y);
        }
        else if (botonDash && dashRestantes > 0)
        {

            rigidbody2.velocity = new Vector2(vector2.x * velocidadDash, vector2.y * velocidadDash);


            StartCoroutine(Espera());

        }


        //Esto podria estar bien, pero habria que hacer que la velocidad de Movimiento tuviera acceleraci�n y rango,
        //y que al tocar un muro, active el flib
        
        if (!EstaEnTierra() && !EstaEnMuro()) 
        {
            rigidbody2.velocity = new Vector2(vectorSaltoMovimiento.x * velocidad, rigidbody2.velocity.y);
        }

        botonSalto = false;






    }

    public void Flip() 
    {

        if (vector2.x > 0)
        { 
            rigidbody2.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        else if (vector2.x < 0) 
        {
            rigidbody2.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
    }


    public IEnumerator Espera() 
    {
        
        yield return new WaitForSeconds(tiempoDash);
        botonDash = false;

    }

    public void Salto() 
    {
        //rigidbody2.velocity = new Vector2(0f, velocidadSalto);
        rigidbody2.AddForce(Vector2.up * velocidadSalto, ForceMode2D.Impulse);
        
        botonSalto = false;
        botonSaltoArriba = false;
    }

    public void SaltoMuro() 
    {
   

        rigidbody2.velocity = new Vector2(fuerzaSaltoMuro.x * -vector2.x,fuerzaSaltoMuro.y);
        StartCoroutine(CambiosSaltosPared());
    }

    public IEnumerator CambiosSaltosPared()
    {
        saltoDeParez = true;
        yield return new WaitForSeconds(tiempoSaltoMuro);
        saltoDeParez = false;
    }

    //Metodo booleano que devuelve si la colision del salto esta tocando la capa del suelo
    public bool EstaEnTierra() 
    {
        return Physics2D.OverlapBox(controladorSuelo.position, dimensionControladorSalto, 0,capaSuelo);
    }

    public bool EstaEnMuro()
    {
        return Physics2D.OverlapBox(controaldorMuro.position, dimensionControladorMuro, 0, capaSuelo);
    }

    
    public void Mover(CallbackContext callbackContext)
    {
        vector2 = callbackContext.ReadValue<Vector2>();
        //Debug.Log(callbackContext.phase);

        if (callbackContext.started) 
        {
            ultimoMovimiento = vector2;
        }
        

    }
    

    public void CambioSwitch(CallbackContext callbackContext) 
    {
        if (callbackContext.started) 
        {
            estadoSwitch = !estadoSwitch;
        }
    }


    public void Jump(CallbackContext callbackContext)
    {

        botonSalto = callbackContext.ReadValueAsButton();
        if (callbackContext.canceled) 
        {
            BotonSaltoArriba();
        }

        if (callbackContext.started) 
        {
            vectorSaltoMovimiento = vector2;
        }

    }

    private void BotonSaltoArriba() 
    {
        if (rigidbody2.velocity.y>0) 
        {
            rigidbody2.AddForce(Vector2.down * rigidbody2.velocity.y * (1-multiplicadorCancelarSalto), ForceMode2D.Impulse );
        }

        botonSaltoArriba = true;
        botonSalto = false;
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
