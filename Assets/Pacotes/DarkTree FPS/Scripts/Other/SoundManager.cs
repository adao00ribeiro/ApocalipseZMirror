using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip pickupSound;
    public AudioClip inventoryOpenSound;
    public AudioClip clickSound;

    public AudioSource source;
    public static SoundManager instance;
    private void Start()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void Pickup()
    {
        source.PlayOneShot(pickupSound);
    }

    public void InventoryOpen()
    {
        source.PlayOneShot(inventoryOpenSound);
    }

    public void Click()
    {
        source.PlayOneShot(clickSound);
    }

}
