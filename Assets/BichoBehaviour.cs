using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BichoBehaviour : MonoBehaviour
{
    bool correr = false;
    public int velocidad = 5;
    public int tiempoVivo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (correr)
        {
            transform.position += new Vector3(velocidad * Time.deltaTime, 0, 0);
            tiempoVivo -= 1;

        }
        if (tiempoVivo < 0)
            Destroy(gameObject);
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            correr = true;
        }
        
    }
}
