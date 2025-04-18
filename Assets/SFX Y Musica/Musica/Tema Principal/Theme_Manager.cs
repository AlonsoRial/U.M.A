using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Theme_Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public float tiempoIntro = 5f; // Duración de la introducción en segundos

    void Start()
    {
        audioSource.Play();
        StartCoroutine(IniciarBucle());
    }

    System.Collections.IEnumerator IniciarBucle()
    {
        yield return new WaitForSeconds(tiempoIntro);
        audioSource.time = tiempoIntro; // Saltar a la parte del bucle
        audioSource.loop = true;
        audioSource.Play();
    }

}
