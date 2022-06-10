using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAudio : MonoBehaviour
{

    [SerializeField]private AudioSource AudioSource;
  


    // Start is called before the first frame update
    void Start ()
    {
        AudioSource = GetComponent<AudioSource> ( );
        AudioSource.Play ( );

    }

}
