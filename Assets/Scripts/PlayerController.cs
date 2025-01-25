using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rigidbody2;
    [SerializeField] private float velocidadMovimiento;


    Vector2 vector2 = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       // vector2.x = Input.GetAxisRaw("Horizontal");
        rigidbody2.transform.position += new Vector3(vector2.x,0,0) * Time.deltaTime * velocidadMovimiento;
    }


    public void Mover(CallbackContext callbackContext)
    {

        vector2 = callbackContext.ReadValue<Vector2>();

        if (vector2.y != 0) 
        {
            Debug.Log(vector2);
        }

    }




}
