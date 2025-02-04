using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;



//Hola Eric, te comento el codigo para que lo entiendas un poco mejor :)
public class PlayerController : MonoBehaviour
{
    // El PlayerInput maneja el nuevo sistema de inputs y la camara, si queremos que el jugador pueda cambiar los controles, se debe de hacer con esto, en teoria...
    PlayerInput playerInput;
    Rigidbody2D rigidbody2;
    Vector2 vector2 = Vector2.zero;

    [Header("MOVIMIENTO")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadSalto;


    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false;


    [Header("SALTO")]
    [SerializeField] private Transform groundTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector2 dimensionHitBoxSalto;
    private bool botonSalto;
    [SerializeField] private int cantidadSaltos;
    [SerializeField] private int saltosRestantes;


    [Header("SALTO MURO")]
    [SerializeField] private Transform muroTransform;
    [SerializeField] private Vector2 dimensionHitBoxSaltoMuro;


    [Header("DASH")]
    [SerializeField] private float velocidadDash;
    [SerializeField] private bool botonDash;
    [SerializeField] private int cantidadDash;
    [SerializeField] private int dashRestantes;
    [SerializeField] private float tiempoDash;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        //Metodo para voltear al personaje al moverse
        Flip();

        //Su finción es que la camara siga al jugadador
        //No se si podras, pero cuando te pongas con la camará, prueba con el playerInput.camara
        //Supuestamete tiene las mismas funciones que solo Camara
        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);


        //Si la hitbox del salto toca el suelo (concretamente la capa "Water", me dio flojera crear una capa nueva), se recarga los saltos y los dashes
        if (EstaEnTierra()) {
            saltosRestantes = cantidadSaltos;
            dashRestantes = cantidadDash;
        }

    }

    //Update para fisicas
    private void FixedUpdate()
    {
        //Si el boton del salto esta activado
        if (botonSalto) 
        {
            //Si esta tocando la capa de tierra el colisionador de salto, llama el metodo de salto
            if (EstaEnTierra()) 
            {
                Salto();
            }//Para hacer el doble salto, si no esta el tierra, el boton de salto vuelve a estar activo (por que el frame del primero es distinto) y tiene saltos restantes, salta otra vez
            else if (!EstaEnTierra() && botonSalto && saltosRestantes>1) 
            {
                Salto();
                saltosRestantes--;
                
            }
        }

        //Si el botón dash no esta activo, el personaje se mueve normal
        if (!botonDash)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadMovimiento, rigidbody2.velocity.y);
        }
        //Si esta activo y le quedan dashes, esté se movera a la velocidad del dash
        else if (botonDash && dashRestantes > 0)
        {
            
            rigidbody2.velocity = new Vector2(vector2.x * velocidadDash, vector2.y * velocidadDash);
            dashRestantes--;
            
            //Llama al "hilo"
            StartCoroutine("Espera");
            
        }

        botonSalto = false;
    }

    //Metodo para voltear el personaje
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

    //Usado para limitar la duración del dash
    //No me ha quedado muy claro, pero es como un hilo, espera el tiempo indicado y luego devuelve false al boton de dash
    public IEnumerator Espera() 
    {
        
        yield return new WaitForSeconds(tiempoDash);
        botonDash = false;

    }

    public void Salto() 
    {
        rigidbody2.velocity = new Vector2(0f, velocidadSalto);
        
        botonSalto = false;
    }

    //Metodo booleano que devuelve si la colision del salto esta tocando la capa del suelo
    public bool EstaEnTierra() 
    {
        return Physics2D.OverlapBox(groundTransform.position, dimensionHitBoxSalto, 0,groundLayer);
    }


    //Todos estos metodos son del apartado de los controles
    //Si pone CallbackContext, son los controles
    //el callbackContext es el valor del input
    
    //Existe una forma de hacerlo sin crear tantos metodos, simplemente creando y llamando a un objeto para que pasé su valor, pero a mi me gustá más asi, da más control
    //Cambiar de una forma a otra no seria un gran ploblema, igual con el antiguo metodo

    public void Mover(CallbackContext callbackContext)
    {
        //Está sería la tradución con el antiguo metodo
        //vector2 = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        vector2 = callbackContext.ReadValue<Vector2>();
    }

    public void CambioSwitch(CallbackContext callbackContext) 
    {
        //bool boton = Input.GetKey(KeyCode.K);
        bool estado = callbackContext.ReadValueAsButton();
        
        //if para que se mantenga el estado booleano, ya que el el input solo reaciona mientras esté pulsado 
        if (!estado) 
        {
            estadoSwitch = !estadoSwitch;
        }

    }


    public void Jump(CallbackContext callbackContext)
    {
        //bool boton = Input.GetKey(KeyCode.W);
        botonSalto = callbackContext.ReadValueAsButton();
    }


    public void Dash(CallbackContext callbackContext)
    {
        //bool boton = Input.GetKey(KeyCode.J);
        botonDash = callbackContext.ReadValueAsButton();
    }

        //Se encarga de dibujar colisiones, de momento solo el salto
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundTransform.position, dimensionHitBoxSalto);

    }

}
