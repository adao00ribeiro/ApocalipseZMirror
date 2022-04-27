using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip clipHouver;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        source.PlayOneShot(clipHouver, 0.5f);
    }

  
}
