using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.ShaderData;
using static UnityEngine.InputSystem.InputAction;


public class PlayerController : MonoBehaviour
{
   

    private Rigidbody2D rigidbody2;
    private Vector2 vector2 = Vector2.zero;
    private PlayerInput playerInput;
    private Vector3 posicionGuardado;



    [Header("MOVIMIENTO")]
    [SerializeField] private float gravedad;
    [SerializeField] private float velocidadX;
    [SerializeField] private float velocidadXCaida;
    [SerializeField] private float gravedadCaida;
    [SerializeField] private float MAX_Velocidad_Caida;
    private bool girando = true;
    private float auxiMove;




    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false;



    [Header("SALTOS")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float fuerzaSaltoDoble;
    [SerializeField] private float fuerzaDetencionSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Vector2 dimensionControladorSalto;
    private bool botonSaltoDoble;
    [SerializeField] private int cantidadSaltosDobles;
    private int saltosDoblesRestantes;




    [Header("SALTO MURO")]
    [SerializeField] private Transform controaldorMuro;
    [SerializeField] private Vector2 dimensionControladorMuro;
    private bool deslizando;
    [SerializeField] private Vector2 fuerzaSaltoPared;
    [SerializeField] private float tiempoSaltoPared;
    private bool saltandoParez;





    [Header("DASH")]
    [SerializeField] private float velocidadDash;
     private bool botonDash;
    [SerializeField] private int cantidadDash;
     private int dashRestantes;
    [SerializeField] private float tiempoDash;
    private int direcionDash;



    [Header("PAUSAR JUEGO")]
    [SerializeField] private Canvas canvas;


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


        if (EstaEnMuro() && !EstaEnTierra() && vector2.x != 0)
        {
            deslizando = true;
        }
        else 
        {
            deslizando= false;
        }


        if (transform.localScale.x>0f) 
        {
            direcionDash = 1;
        }
        else 
        {
            direcionDash = -1;
        }

    }

    


    private void FixedUpdate()
    {

        if (!saltandoParez)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadX, rigidbody2.velocity.y);
        }

        if (rigidbody2.velocity.y < 0)
        {
            rigidbody2.velocity = new Vector2(velocidadXCaida * vector2.x, rigidbody2.velocity.y);
        }

        Debug.Log(rigidbody2.velocity.x);



        //DASH
        if (botonDash && dashRestantes > 0)
        {
            rigidbody2.velocity = new Vector2(direcionDash * velocidadDash, 0);

            StartCoroutine(Espera());

        }

        if (deslizando) 
        {
            
            rigidbody2.velocity = new Vector2 (rigidbody2.velocity.x, Mathf.Clamp(rigidbody2.velocity.y, -0, float.MaxValue));
        }


        if (rigidbody2.velocity.y < 0)
        {
            rigidbody2.gravityScale += Time.deltaTime * gravedadCaida;

        }

        if (rigidbody2.velocity.y < MAX_Velocidad_Caida)
        {

            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, MAX_Velocidad_Caida);
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
        auxiMove = callbackContext.ReadValue<Vector2>().x;

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

        if (callbackContext.started && EstaEnTierra()) 
        {
          
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, fuerzaSalto);
            
        }

        if (callbackContext.canceled && rigidbody2.velocity.y > 0) 
        {
           
            rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, rigidbody2.velocity.y/fuerzaDetencionSalto);
            

        }

        if (callbackContext.canceled) 
        {
            botonSaltoDoble = true; 
        }

        if (callbackContext.started && botonSaltoDoble && saltosDoblesRestantes!=0) 
        {
            //Esto con el salto muro falla un poco
            rigidbody2.velocity = new Vector2(0, 0);
          
            rigidbody2.AddForce(new Vector2(rigidbody2.velocity.x, fuerzaSaltoDoble), ForceMode2D.Impulse);
            botonSaltoDoble = false;
            saltosDoblesRestantes--;
        }



        if (callbackContext.started && EstaEnMuro() && deslizando) 
        {
            Debug.Log("MUROOOO");
            rigidbody2.velocity = new Vector2(fuerzaSaltoPared.x * -vector2.x, fuerzaSaltoPared.y);
            StartCoroutine(TiempoSaltoParez());
        }

    }


    IEnumerator TiempoSaltoParez() 
    {
        saltandoParez = true;

      
        vector2.x = -vector2.x;

        if (EstaEnMuro()) yield return true;
      
        yield return new WaitForSeconds(tiempoSaltoPared);

        saltandoParez = false;
        vector2.x = auxiMove;

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
            Time.timeScale = 0;
            canvas.enabled = true;
            this.gameObject.SetActive(false);
          
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
