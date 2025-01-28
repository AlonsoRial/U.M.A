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
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float velocidadSalto;


    Vector2 vector2 = Vector2.zero;

    public Transform groundTransform;
    public LayerMask groundLayer;
    public Vector2 dimensionHitBoxSalto;

    public bool estadoSwitch = false;
    private int contador = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {

        if (estadoSwitch)
        {
            Debug.Log("ROJO");
        }
        else 
        {
            Debug.Log("AZUL");
        }

       // vector2.x = Input.GetAxisRaw("Horizontal");
       
        // el transform positition es de tontos
       // rigidbody2.transform.position += new Vector3(vector2.x,0,0) * Time.deltaTime * velocidadMovimiento;

        rigidbody2.velocity = new Vector2 (vector2.x * velocidadMovimiento, rigidbody2.velocity.y);

        playerInput.camera.transform.position = new Vector3(rigidbody2.position.x, rigidbody2.position.y, -10);

        if (vector2.y > 0) 
        {
            if (EstaEnTierra()) 
            {
                rigidbody2.velocity = new Vector2(rigidbody2.velocity.x, velocidadSalto);
            }
        }

    }

    public bool EstaEnTierra() 
    {
        return Physics2D.OverlapBox(groundTransform.position, dimensionHitBoxSalto, 0,groundLayer);
    }

    public void Mover(CallbackContext callbackContext)
    {

        vector2 = callbackContext.ReadValue<Vector2>();

        if (vector2.y != 0) 
        {
           // Debug.Log(vector2);
        }

    }

    public void CambioSwitch(CallbackContext callbackContext) 
    {
        bool estado = callbackContext.ReadValueAsButton();
        

        if (!estado) 
        {
            contador++;
        }

        if (contador%2==0)
        {
            estadoSwitch = true;
            
        }
        else 
        {
            estadoSwitch = false;
            
        }
    
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundTransform.position, dimensionHitBoxSalto);

    }




}
