using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoBehaviour : MonoBehaviour
{
    public int velocidad = 5;
    bool corre;
    public int tiempoVivo;
    Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tiempoVivo < 0)
            Destroy(gameObject);
            
        if (corre)
            tiempoVivo -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb.velocity = new Vector2(velocidad, 0);
            corre = true;
            animator.SetTrigger("Visto");
        }
        
    }
}
