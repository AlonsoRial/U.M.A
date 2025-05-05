using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEditor.ShaderData;
using static UnityEngine.InputSystem.InputAction;


public class PlayerController : MonoBehaviour
{


    private Rigidbody2D rigidbody2; //el personaje que el jugador controla
    private Vector2 vector2 = Vector2.zero; //El valor input que envia el jugador
    private PlayerInput playerInput; // La cámara persigue el jugador
    private Vector3 posicionGuardado; //guarda la posición del checkpoint
    private CapsuleCollider2D capsuleCollider2D;


    [Header("MOVIMIENTO")]
    [SerializeField] private float gravedad; // gravedad normal general
    [SerializeField] private float velocidadX; //su velocidad
    [SerializeField] private float velocidadXCaida; //su velocidad cuando el jugador esté cayendo
    [SerializeField] private float gravedadCaida;// lo que aumentará su gravedad normal cuando esté cayendo
    [SerializeField] private float MAX_Velocidad_Caida; // la maxima gravedad que puede alcanzar
    private bool girando = true; //booleano que dice en que lado está mirando el jugador


    CallbackContext _dash;


    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false; //el estado del switch

    [Header("Hazards")]
    [SerializeField] private float tiempoMuerte;



    [Header("SALTOS")]
    [SerializeField] private float fuerzaSalto; //fuerza del salto principal
    [SerializeField] private Transform controladorSuelo; //el objeto que hace de hitbox del salto, debe de ir en los pies del personaje
    [SerializeField] private Vector2 dimensionControladorSalto; //El tamaño de la hitbox del salto
    [SerializeField] private LayerMask capaSuelo; //la capa donde el jugador pueda saltar
    [SerializeField] private int cantidadSaltosDobles; //los saltos dobles / secundarios que puede hacer el jugador
    [SerializeField] private int saltosDoblesRestantes; // la cantidad de saltos dobles que el jugador puede ejercer
    [SerializeField] float recuperacionSalto;



    [Header("SALTO MURO")]
    [SerializeField] private Transform controaldorMuro; //La hitbox cuando choca con un muro
    [SerializeField] private Vector2 dimensionControladorMuro; //el tamaño de dicha hitbox
    private bool enMuro; //si el personaje esta chocando con el muro a la vez que esta cayendo
    [SerializeField] private Vector2 fuerzaSaltoPared; //la fuerza que tiene el salto de la parez
    [SerializeField] private float tiempoSaltoPared; //el tiempo que se debe de esperar para que al jugador le devuelvan los controles
    private bool saltandoParez; //booleano que indica si la acción del salto de la parez ha sido activada 
    private float auxiMove; // guarda el movimiento del jugador para cuando retome el control del movimiento


    [Header("DASH")]
    [SerializeField] private float velocidadDash; //velocidad del dash
    private bool estaenDash = false;
    private int direcionDash;
    private bool puedeDash = true;
    [SerializeField] private float tiempoDash;


    [Header("PAUSAR JUEGO")]
    [SerializeField] private Canvas canvas; //El Canvas para Pausar el juego

    private Animator animator;

    private int controlAniX, controlAniY;

    public bool EstadoSwitch { get => estadoSwitch; } //Get del estadoSwitch para que el codigo SwitchScript pueda haceder a él

  
    [SerializeField] private Audio_Script _audio;
    

    Vector3 escalaLocal;



    private void Start()
    {
        _audio = FindObjectOfType<Audio_Script>();

        if (_audio == null)
        {
            Debug.LogError("No se encontró el script Audio_Script en la escena.");
        }
     
    }

    // Start is called before the first frame update
    void Awake()
    {

        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        vector2.x = 0;
        rigidbody2.velocity = Vector3.zero;
        InvokeRepeating(nameof(SonidosAlAndar), 0, _audio.TiempoRepeticionPasos);
    }

    //Metodo que se encarga del giro del jugador
    public void Flip()
    {
        if (girando && vector2.x < 0f || !girando && vector2.x > 0f)
        {
            girando = !girando;
            escalaLocal = transform.localScale;
            escalaLocal.x *= -1f;
            transform.localScale = escalaLocal;

        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Menu.Segundos<0.0f) 
        {
            Muerte();
            Menu.Segundos = Menu.TiempoInicial;
        }

        //Metodo del giro del jugador
        Flip();

        //codigo para que la camará siga al jugador
        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);

        /*
        //Resetea la cantidad de dashes cuando cumpla estas condiciones
        if (EstaEnTierra() || (!EstaEnTierra() && EstaEnMuro() && vector2.x != 0))
        {
            dashRestantes = cantidadDash;

        }
        */

        controlAniX = rigidbody2.velocity.x !=0.0f && EstaEnTierra() ? 1:0;
        controlAniY = rigidbody2.velocity.y != 0.0f && !EstaEnTierra() ? 1 : 0;

        animator.SetInteger("V_X",controlAniX);
       // animator.SetInteger("V_Y", controlAniY);
        animator.SetBool("EnMuro", EstaEnMuro());
      

        //si el jugador está tocando el suelo, se resetea la gravedad y los dobles saltos
        if (EstaEnTierra())
        {
            rigidbody2.gravityScale = gravedad;
            saltosDoblesRestantes = cantidadSaltosDobles;
        }

        //detecta si el jugador está cayendo mientras toca el muro
        enMuro = EstaEnMuro() && !EstaEnTierra() && vector2.x != 0 ? true : false;
        direcionDash = transform.localScale.x > 0f ? 1 : -1;

        animator.SetBool("EnTierra",EstaEnTierra());

    }

    public void SonidosAlAndar() 
    {
        

        if (EstaEnTierra() && rigidbody2.velocity.x !=0) 
        {
            _audio.AudioPasos.PlayOneShot(_audio.AudioPasos.clip);
        }
        
    }


    public void DetenerDash() 
    {
        estaenDash = false;
        StartCoroutine(hiloDash());
    }

    IEnumerator hiloDash() 
    {
        yield return new WaitForSeconds(tiempoDash);
        puedeDash = true;
    }


    private void FixedUpdate()
    {
        
            //Velocidad normal del jugador mientras no esté saltando en la parez
            if (!saltandoParez)
            {
                rigidbody2.velocity = new Vector2(vector2.x * velocidadX, rigidbody2.velocity.y);
            }


            if (rigidbody2.velocity.y < 0)
            {
                rigidbody2.velocity = new Vector2(velocidadXCaida * vector2.x, rigidbody2.velocity.y); //la velocidad horizontal cuando el jugador caiga
                rigidbody2.gravityScale += Time.deltaTime * gravedadCaida; //se le suma la gravedad
            }



            if (estaenDash)
            {
                //rigidbody2.AddForce(new Vector2(direcionDash* velocidadDash,0));
                rigidbody2.velocity = new Vector2(direcionDash * velocidadDash, 0);
            }


            //Metodo para que el jugador se paré cuando toque el muro para el salto de parez
            if (enMuro)
            {

                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, Mathf.Clamp(rigidbody2.velocity.y, -0, float.MaxValue));
            }

            //si la velocidad de caida sobrepasa a la maxima, la maxima velocidad de caida será la velocidad de caida
            if (rigidbody2.velocity.y < MAX_Velocidad_Caida)
            {

                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, MAX_Velocidad_Caida);
            }

  
    }

    public void Caida()
    {
        _audio.AudioAterrizaje.PlayOneShot(_audio.AudioAterrizaje.clip);
    }


    //Función que devuelve si el jugador está tocando el suelo o no
    public bool EstaEnTierra()
    {
        return Physics2D.OverlapBox(controladorSuelo.position, dimensionControladorSalto, 0, capaSuelo);
    }

    //Función que devuelve si el jugador está tocando el muro o no
    public bool EstaEnMuro()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(controaldorMuro.position, dimensionControladorMuro, 0);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("PARKOUR") || col.CompareTag("SWITCH-VERDE") || col.CompareTag("SWITCH-ROJO"))
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
            //source.PlayOneShot(sonidoSwitch);
            _audio.AudioSwitch.PlayOneShot(_audio.AudioSwitch.clip);
            estadoSwitch = !estadoSwitch;
        }
    }

    public void SonidoExplision() 
    {
        _audio.AudioMuerte.PlayOneShot(_audio.AudioMuerte.clip);
    }

    public void Salto() 
    {
        //source.PlayOneShot(sonidoSalto);
        _audio.AudioSalto.PlayOneShot(_audio.AudioSalto.clip);
        rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, fuerzaSalto);
    }

    //Salto
    public void Jump(CallbackContext callbackContext)
    {
        if (EstaEnTierra())
        {
            if (callbackContext.performed)
            {
                animator.SetTrigger("T_Salto");
            }

        }
        else 
        {
            if (callbackContext.performed && saltosDoblesRestantes > 0 && !EstaEnMuro()) 
            {
                rigidbody2.gravityScale = gravedad;
                animator.SetTrigger("T_D_Salto");
                saltosDoblesRestantes--;
            }

        }

        if (callbackContext.started) 
        {
            estaenDash = false;
        }

     
        if (callbackContext.started && EstaEnMuro() && enMuro)
        {
            //la velocidad pasa a ser a la de la fuerza del salto, se le invierte el input de horizontal para el giro del jugador
            rigidbody2.velocity = new Vector2(fuerzaSaltoPared.x * -vector2.x, fuerzaSaltoPared.y);
            StartCoroutine(TiempoSaltoParez()); //llamada al hilo
            rigidbody2.gravityScale = gravedad;
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
        _dash = callbackContext;
        
        if (callbackContext.started && puedeDash) 
        {
            //source.PlayOneShot(sonidoDash);
            _audio.AudioDash.PlayOneShot(_audio.AudioDash.clip);
            puedeDash = false;
            animator.SetTrigger("T_Dash");
            estaenDash = true;
            
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

    private CheckPoint_Script check;


    public void Muerte() 
    {
        capsuleCollider2D.enabled = false;
        playerInput.enabled = false;
        animator.SetTrigger("Muerte");

        rigidbody2.constraints = RigidbodyConstraints2D.FreezePosition;

        rigidbody2.gravityScale = 0;
    }

    //Las colisiones
    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si choca con un hazards, el jugador pasa a ser teletransportado al checkpoint
        if (collision.gameObject.CompareTag("HAZARDS"))
        {

            Muerte();

        }

        //Si choca un checkpoint, ese será el nuevo checkpoint
        if (collision.gameObject.CompareTag("CHECKPOINT") )
        {   
            
            check = collision.gameObject.GetComponent<CheckPoint_Script>();
           

            posicionGuardado = collision.transform.position;

          
        }

        if (collision.gameObject.CompareTag("NO_CONTROL")) 
        {
            
            playerInput.enabled = false;
            vector2.x = 1;

          
            
        }

        if (collision.gameObject.CompareTag("ANI"))
        {

            Debug.Log("hola soy Rodolfo Mascarpone, igual el que te lo quita como el que te lo pone");
            gameObject.SetActive(false);
          


        }


    }


    public void RespawnTime() {
       
        _audio.AudioRespawn.PlayOneShot(_audio.AudioRespawn.clip);
        capsuleCollider2D.enabled = true;
        rigidbody2.transform.position = posicionGuardado;
        playerInput.enabled = true;
        rigidbody2.constraints = RigidbodyConstraints2D.None;
        rigidbody2.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidbody2.transform.eulerAngles = Vector3.zero;
        rigidbody2.gravityScale = gravedad;
        animator.SetTrigger("T_Respawn");
        check.AniParticulas();

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionControladorSalto); //Dibuja de Rojo la hitbox del salto

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(controaldorMuro.position, dimensionControladorMuro); //Dibuja de verde la hitbox del muro

    }


}
