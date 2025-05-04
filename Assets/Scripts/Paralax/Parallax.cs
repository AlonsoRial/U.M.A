using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    private float startpositionX;
    private float startpositionY;
    public GameObject camera;
    public float parallaxEffect;
    public float parallaxSpeed;
   
    
    void Start()
    {
        startpositionX = transform.position.x;
        startpositionY = transform.position.y;

    }

    // Update is called once per frame
    void FixUpdate()
    {
        float distanceX = camera.transform.position.x  * parallaxEffect; // 0 = se mueve con la camara || 1 = no se mueve con la camara

        transform.position = new Vector3(startpositionX + distanceX, transform.position.y, transform.position.z);

        //float distanceY = camera.transform.position.y * parallaxEffect; // 0 = se mueve con la camara || 1 = no se mueve con la camara

        //transform.position = new Vector3(startpositionY + distanceY, startpositionY + distanceY);



    }
}
