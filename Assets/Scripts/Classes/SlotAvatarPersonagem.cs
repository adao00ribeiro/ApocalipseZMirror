using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SlotAvatarPersonagem : MonoBehaviour
{
    public static SlotAvatarPersonagem selecionado;
    public int IdPersonagem;
    public Image ImageAvatar;
    public Image ImageCadeado;
    public Button ButtonComprar;
    public Text AP;
    public bool TemPersonagem;
      
    private void Start()
    {
       GetComponent<Button>().onClick.AddListener(InstanciaPersonagem);
    }
    public void Inicializa(int _IdPersonagem, Sprite avatar,int ap)
    {
        IdPersonagem = _IdPersonagem;
        ImageAvatar.sprite = avatar;
        AP.text = "AP: " + ap.ToString();
        Prepara();
    }

    public void Prepara()
    {
        if (TemPersonagem)
        {
            GetComponent<Button>().interactable = true;
            ImageCadeado.gameObject.SetActive(false);
            ButtonComprar.gameObject.SetActive(false);
            return;
        }
        GetComponent<Button>().interactable = false;
        ImageCadeado.gameObject.SetActive(true);
        ButtonComprar.gameObject.SetActive(true);
    }


    public void InstanciaPersonagem()
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;
        if (spawPoint.childCount>0)
        {
            Destroy(spawPoint.GetChild(0).gameObject);
        }

        GameObject.FindObjectOfType<ScriptableManager>().InstanciaPersonagem(IdPersonagem, spawPoint);
    }
}
