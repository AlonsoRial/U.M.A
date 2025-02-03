using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
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

    [Header("Dash")]
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
        Flip();
        // vector2.x = Input.GetAxisRaw("Horizontal");

        // el transform positition es de tontos
        // rigidbody2.transform.position += new Vector3(vector2.x,0,0) * Time.deltaTime * velocidadMovimiento;


        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);



        if (EstaEnTierra()) {
            saltosRestantes = cantidadSaltos;
            dashRestantes = cantidadDash;
        }



    }


    private void FixedUpdate()
    {

        if (botonSalto) 
        {
            if (EstaEnTierra()) 
            {
                Salto();
            }
            else if (!EstaEnTierra() && botonSalto && saltosRestantes>1) 
            {
                Salto();
                saltosRestantes--;
                
            }
        }


        if (!botonDash)
        {
            rigidbody2.velocity = new Vector2(vector2.x * velocidadMovimiento, rigidbody2.velocity.y);
        }
        else if (botonDash && dashRestantes > 0)
        {
            //rigidbody2.velocity = vector2 * velocidadDash * velocidadMovimiento;
            rigidbody2.velocity = new Vector2(vector2.x * velocidadDash, vector2.y * velocidadDash);
            dashRestantes--;
            
            StartCoroutine("Espera");
            
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
        rigidbody2.velocity = new Vector2(0f, velocidadSalto);
        
        botonSalto = false;
    }

    public bool EstaEnTierra() 
    {
        return Physics2D.OverlapBox(groundTransform.position, dimensionHitBoxSalto, 0,groundLayer);
    }

    public void Mover(CallbackContext callbackContext)
    {

        vector2 = callbackContext.ReadValue<Vector2>();

        //Debug.Log(vector2);

    }

    public void CambioSwitch(CallbackContext callbackContext) 
    {
        bool estado = callbackContext.ReadValueAsButton();
        
        if (!estado) 
        {
            estadoSwitch = !estadoSwitch;
        }

    }


    public void Jump(CallbackContext callbackContext)
    {
        botonSalto = callbackContext.ReadValueAsButton();
    }


    public void Dash(CallbackContext callbackContext)
    {
        botonDash = callbackContext.ReadValueAsButton();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundTransform.position, dimensionHitBoxSalto);

    }

}
