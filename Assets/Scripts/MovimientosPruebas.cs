using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;
using TMPro;

public class MovimientosPruebas : MonoBehaviour
{


    private Rigidbody2D rigidbody2; //el personaje que el jugador controla
    private Vector2 vector2 = Vector2.zero; //El valor input que envia el jugador
    private PlayerInput playerInput; // La c�mara persigue el jugador
    private Vector3 posicionGuardado; //guarda la posici�n del checkpoint



    [Header("MOVIMIENTO")]
    [SerializeField] private float gravedad; // gravedad normal general
    [SerializeField] private float velocidadX; //su velocidad
    [SerializeField] private float velocidadXCaida; //su velocidad cuando el jugador est� cayendo
    [SerializeField] private float gravedadCaida;// lo que aumentar� su gravedad normal cuando est� cayendo
    [SerializeField] private float MAX_Velocidad_Caida; // la maxima gravedad que puede alcanzar
    private bool girando = true; //booleano que dice en que lado est� mirando el jugador





    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false; //el estado del switch

    [Header("Hazards")]
    [SerializeField] private float tiempoMuerte;



    [Header("SALTOS")]
    [SerializeField] private float fuerzaSalto; //fuerza del salto principal
    [SerializeField] private float fuerzaSaltoDoble; //fuerza del doble salto o salto secundar�o
    [SerializeField] private float fuerzaDetencionSalto; // para hacer el salto regulable
    [SerializeField] private Transform controladorSuelo; //el objeto que hace de hitbox del salto, debe de ir en los pies del personaje
    [SerializeField] private Vector2 dimensionControladorSalto; //El tama�o de la hitbox del salto
    [SerializeField] private LayerMask capaSuelo; //la capa donde el jugador pueda saltar
    private bool botonSaltoDoble; //booleado que indica si se ha activado la acci�n de doble salto
    [SerializeField] private int cantidadSaltosDobles; //los saltos dobles / secundarios que puede hacer el jugador
    private int saltosDoblesRestantes; // la cantidad de saltos dobles que el jugador puede ejercer




    [Header("SALTO MURO")]
    [SerializeField] private Transform controaldorMuro; //La hitbox cuando choca con un muro
    [SerializeField] private Vector2 dimensionControladorMuro; //el tama�o de dicha hitbox
    private bool enMuro; //si el personaje esta chocando con el muro a la vez que esta cayendo
    [SerializeField] private Vector2 fuerzaSaltoPared; //la fuerza que tiene el salto de la parez
    [SerializeField] private float tiempoSaltoPared; //el tiempo que se debe de esperar para que al jugador le devuelvan los controles
    private bool saltandoParez; //booleano que indica si la acci�n del salto de la parez ha sido activada 
    private float auxiMove; // guarda el movimiento del jugador para cuando retome el control del movimiento





    [Header("DASH")]
    [SerializeField] private float velocidadDash; //velocidad del dash
    private bool botonDash; //acci�n del dash
    [SerializeField] private int cantidadDash; //cantidad de dashes
    private int dashRestantes; //dashes restantes del juador
    [SerializeField] private float tiempoDash; //tiempo que dura el dash
    private int direcionDash; //La direcci�n el cual se har� el dash



    [Header("PAUSAR JUEGO")]
    [SerializeField] private Canvas canvas; //El Canvas para Pausar el juego


    public bool EstadoSwitch { get => estadoSwitch; } //Get del estadoSwitch para que el codigo SwitchScript pueda haceder a �l


    //Metodo que se encarga del giro del jugador
    public void Flip()
    {
        if (girando && vector2.x < 0f || !girando && vector2.x > 0f)
        {
            girando = !girando;
            Vector3 escalaLocal = transform.localScale;
            escalaLocal.x *= -1f;
            transform.localScale = escalaLocal;
            
        }

    }


    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        

    }

    // Update is called once per frame
    void Update()
    {
        //Metodo del giro del jugador
        Flip();

        //codigo para que la camar� siga al jugador
        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);


        //Resetea la cantidad de dashes cuando cumpla estas condiciones
        if (EstaEnTierra() || (!EstaEnTierra() && EstaEnMuro() && vector2.x != 0))
        {
            dashRestantes = cantidadDash;

        }



        //si el jugador est� tocando el suelo, se resetea la gravedad y los dobles saltos
        if (EstaEnTierra())
        {
            rigidbody2.gravityScale = gravedad;
            botonSaltoDoble = false;
            saltosDoblesRestantes = cantidadSaltosDobles;
        }

        //detecta si el jugador est� cayendo mientras toca el muro
        enMuro = EstaEnMuro() && !EstaEnTierra() && vector2.x != 0 ? true : false;
        /*
        if (EstaEnMuro() && !EstaEnTierra() && vector2.x != 0)
        {
            enMuro = true;
        }
        else 
        {
            enMuro= false;
        }
        */

        //calcula diraci�n del dash
        direcionDash = transform.localScale.x > 0f ? 1 : -1;
        /*
        if (transform.localScale.x>0f) 
        {
            direcionDash = 1;
        }
        else 
        {
            direcionDash = -1;
        }
        */

    }




    private void FixedUpdate()
    {
        //Velocidad normal del jugador mientras no est� saltando en la parez
        if (!saltandoParez)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadX, rigidbody2.velocity.y);
        }


        if (rigidbody2.velocity.y < 0)
        {
            rigidbody2.velocity = new Vector2(velocidadXCaida * vector2.x, rigidbody2.velocity.y); //la velocidad horizontal cuando el jugador caiga
            rigidbody2.gravityScale += Time.deltaTime * gravedadCaida; //se le suma la gravedad
        }





        //DASH
        if (botonDash && dashRestantes > 0)
        {
            rigidbody2.velocity = new Vector2(direcionDash * velocidadDash, 0);

            StartCoroutine(Espera());

        }

        //Metodo para que el jugador se par� cuando toque el muro para el salto de parez
        if (enMuro)
        {

            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, Mathf.Clamp(rigidbody2.velocity.y, -0, float.MaxValue));
        }

        //si la velocidad de caida sobrepasa a la maxima, la maxima velocidad de caida ser� la velocidad de caida
        if (rigidbody2.velocity.y < MAX_Velocidad_Caida)
        {

            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, MAX_Velocidad_Caida);
        }

    }

    //Tiempo de espera del dash, una vez cumplido podr� hacer otro dash
    public IEnumerator Espera()
    {
        yield return new WaitForSeconds(tiempoDash);
        botonDash = false;

    }

    //Funci�n que devuelve si el jugador est� tocando el suelo o no
    public bool EstaEnTierra()
    {
        return Physics2D.OverlapBox(controladorSuelo.position, dimensionControladorSalto, 0, capaSuelo);
    }

    //Funci�n que devuelve si el jugador est� tocando el muro o no
    public bool EstaEnMuro()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(controaldorMuro.position, dimensionControladorMuro, 0);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("PARKOUR")) 
            {
                return true;
            }
        }

        return false;
        // return Physics2D.OverlapBox(controaldorMuro.position, dimensionControladorMuro, 0, capaMuroSaltable);
    }


    /*ZONA DE INPUTS*/

    //recibe el input del movimiento en horizontal
    public void Mover(CallbackContext callbackContext)
    {
        vector2.x = callbackContext.ReadValue<Vector2>().x;
        auxiMove = callbackContext.ReadValue<Vector2>().x;

    }

    //el cambio del switch
    public void CambioSwitch(CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            estadoSwitch = !estadoSwitch;
        }
    }

    //Salto
    public void Jump(CallbackContext callbackContext)
    {
        //Llamada al salto normal
        if (callbackContext.started && EstaEnTierra())
        {
            //EN TEORIA, AQUI SE LLAMA LA ANIMACION DE PREPARACION
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, fuerzaSalto);

        }

        //Regula el salto apra hacerlo regulable
        if (callbackContext.canceled && rigidbody2.velocity.y > 0)
        {

            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, rigidbody2.velocity.y / fuerzaDetencionSalto);


        }

        //cuando el jugador deja de presionar el bot�n de salto, la acci�n de doble salto estar� disponible
        if (callbackContext.canceled)
        {
            botonSaltoDoble = true;
        }

        //una vez ejecutado el doble salto
        if (callbackContext.started && botonSaltoDoble && saltosDoblesRestantes != 0)
        {
            //EN TEORIA, AQUI SE LLAMA LA ANIMACION DE PREPARACION
            rigidbody2.velocity = new Vector2(0, 0); //la velocidad pasar� a ser 0

            rigidbody2.AddForce(new Vector2(rigidbody2.velocity.x, fuerzaSaltoDoble), ForceMode2D.Impulse); // se le aplica una fuerza para la nueva velocidad
            botonSaltoDoble = false; //deja  de poder hacer el doble salto
            saltosDoblesRestantes--; //un salto menos
        }


        //Llamada del salto en la parez
        if (callbackContext.started && EstaEnMuro() && enMuro)
        {
            //la velocidad pasa a ser a la de la fuerza del salto, se le invierte el input de horizontal para el giro del jugador
            rigidbody2.velocity = new Vector2(fuerzaSaltoPared.x * -vector2.x, fuerzaSaltoPared.y);
            StartCoroutine(TiempoSaltoParez()); //llamada al hilo
        }

    }


    IEnumerator TiempoSaltoParez()
    {
        saltandoParez = true; //esta saltando n la parez, para que no intervenga el movimiento normal


        vector2.x = -vector2.x; //el valor del input horizontal se invierte

        if (EstaEnMuro()) yield return true; //si toca con otro muro, el hilo termina

        yield return new WaitForSeconds(tiempoSaltoPared); //si el tiempo termina, el hilo termina

        //cuanto el hilo termina, todo vuelve a la normalidad
        saltandoParez = false;
        vector2.x = auxiMove;

    }



    public void Dash(CallbackContext callbackContext)
    {
        //si llama al dash, est� puede hacer un dash
        if (callbackContext.started)
        {
            botonDash = true;

        }

        //si levanta el boton de dash, est� se restar� .
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
        //Al pausar el juego, el tiempo se detiene, el jugador de oculta y muestra el canvas
        if (callbackContext.started)
        {
            Time.timeScale = 0;
            canvas.enabled = true;
            this.gameObject.SetActive(false);

        }

    }

    //Las colisiones
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si choca con un hazards, el jugador pasa a ser teletransportado al checkpoint
        if (collision.gameObject.CompareTag("HAZARDS"))
        {
            Debug.Log("muerte");
            playerInput.enabled = false;
            StartCoroutine(RespawnTime());
        }

        //Si choca un checkpoint, ese ser� el nuevo checkpoint
        if (collision.gameObject.CompareTag("CHECKPOINT"))
        {
            Debug.Log("checkpoint guardado");

            posicionGuardado = collision.transform.position;

        }
    }


    IEnumerator RespawnTime()
    {

        yield return new WaitForSeconds(tiempoMuerte);
        rigidbody2.transform.position = posicionGuardado;
        playerInput.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionControladorSalto); //Dibuja de Rojo la hitbox del salto

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(controaldorMuro.position, dimensionControladorMuro); //Dibuja de verde la hitbox del muro

    }


}

