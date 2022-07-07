using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField]private AudioSource audioSource;
    private static Sound _instance;
    public static Sound Instance
    {
        get
        {

            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if ( _instance != null && _instance != this )
        {
            Destroy ( this.gameObject );
        }
        else
        {
            _instance = this;
        }
        audioSource = GetComponent<AudioSource> ( );
    }

    public void PlayOneShot (Vector3 position ,  AudioClip clip)
    {
        transform.position = position;
        audioSource.PlayOneShot ( clip );
    }
    public void Play ( Vector3 position , AudioClip clip )
    {
        transform.position = position;
        audioSource.clip = clip;
        audioSource.Play (  );
    }

}
