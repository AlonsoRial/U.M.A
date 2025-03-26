using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    RaycastHit2D hit;
    public int radio = 5;
    public int life = 5;
    Animator animator;
    public GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("vida",life);

        Detecion();
    }


    void Destrucion() 
    {
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetTrigger("danyo");
            life--;
            Debug.Log("FGDGDSFGSDFGSFGSfg");
        }
 
    }

    public void Detecion() 
    {
        hit = Physics2D.CircleCast(transform.position, radio, Vector2.zero, 0, player.gameObject.layer);
        if (hit) Debug.Log("fdsa");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,radio);


    }

}
