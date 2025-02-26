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
    [SerializeField] private float velocidadMovimiento;




    [Header("SWITCH")]
    [SerializeField] private bool estadoSwitch = false;



    [Header("SALTOS")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private int cantidadSaltos;
    [SerializeField] private int saltosRestantes;
    private bool botonSalto;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Vector2 dimensionControladorSalto;





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

        if (vector2.x > 0)
        {
            rigidbody2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (vector2.x < 0)
        {
            rigidbody2.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

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

        Flip();


        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);


        if (EstaEnTierra() || (!EstaEnTierra() && EstaEnMuro() && vector2.x != 0))
        {
            saltosRestantes = cantidadSaltos;
            dashRestantes = cantidadDash;

        }


    }


    private void FixedUpdate()
    {
   
 

 


        //Movimiento y Dash
        if (!botonDash && !EstaEnMuro())
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadMovimiento, rigidbody2.velocity.y);
        }
        else if (botonDash && dashRestantes > 0)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadDash, vector2.y * velocidadDash);

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
        vector2 = callbackContext.ReadValue<Vector2>();
        //Debug.Log(callbackContext.phase);

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

       // botonSalto = callbackContext.ReadValueAsButton();

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
