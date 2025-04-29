using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Script : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource audioTema;

    [SerializeField] private AudioSource audioPasos;
    [SerializeField] private float tiempoRepeticionPasos;

    [SerializeField] private AudioSource audioSalto;
    [SerializeField] private AudioSource audioDash;
    [SerializeField] private AudioSource audioSwitch;
    [SerializeField] private AudioSource audioAterrizaje;
    [SerializeField] private AudioSource audioMuerte;
    [SerializeField] private AudioSource audioRespawn;

    [SerializeField] private AudioSource[] efectos;



    public AudioSource AudioPasos { get => audioPasos; }
    public AudioSource AudioSalto { get => audioSalto; }
    public AudioSource AudioDash { get => audioDash;  }
    public AudioSource AudioSwitch { get => audioSwitch; }
    public AudioSource AudioAterrizaje { get => audioAterrizaje; }
    public AudioSource AudioMuerte { get => audioMuerte; }
    public AudioSource AudioRespawn { get => audioRespawn; }
    public AudioSource AudioTema { get => audioTema; }
    public float TiempoRepeticionPasos { get => tiempoRepeticionPasos;  }
    //bv

    

    public void CambiarVolumenEfectos(float valor)
    {
        // print("Volumen efectos: " +  valor);


        foreach (AudioSource audio in efectos) 
        {
            audio.volume = valor;
           
        }

    }


}
