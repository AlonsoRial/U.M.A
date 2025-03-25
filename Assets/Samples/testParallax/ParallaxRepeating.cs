using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxRepeating : MonoBehaviour
{
    private float spriteWidth;
    private Transform cam;

    public float parallaxMultiplier; // Controla la velocidad del parallax

    void Start()
    {
        cam = Camera.main.transform;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Movimiento Parallax
        transform.position += new Vector3((cam.position.x - transform.position.x) * parallaxMultiplier, 0, 0);

        // Si la cámara se aleja demasiado, reposiciona el sprite
        if (cam.position.x >= transform.position.x + spriteWidth)
        {
            transform.position += new Vector3(spriteWidth * 2, 0, 0);
        }
        else if (cam.position.x <= transform.position.x - spriteWidth)
        {
            transform.position -= new Vector3(spriteWidth * 2, 0, 0);
        }
    }
}
