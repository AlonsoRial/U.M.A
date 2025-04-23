using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoBehaviour : MonoBehaviour
{
    [SerializeField] private int velocidad = 5;
    [SerializeField] private float tiempoVivo;
    private Rigidbody2D rb;
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private bool desaparicion = false;

    private int posicion;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        posicion = transform.localScale.x > 0 ? 1 : -1;
    }

    // Update is called once per frame

    private void Update()
    {
        if (desaparicion) 
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, tiempoVivo -= Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb.velocity = new Vector2(velocidad * posicion, 0);
            animator.SetTrigger("Visto");
            StartCoroutine(DuracionVida());

            desaparicion = true;

           
        }
        
    }

    IEnumerator DuracionVida() 
    {
        yield return new WaitForSeconds(tiempoVivo);
        Destroy(gameObject);
    }

}
