using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SlotAvatarSelecionado : MonoBehaviour
{
    [SerializeField] private int idPersonagem ;
    [SerializeField] private Image Imagem;
    [SerializeField] private Button button;

    void Awake()
    {
        idPersonagem = -1;
        Imagem = transform.GetChild(0).GetComponent<Image>();
        button = GetComponent<Button>();
       
    }

    public void AddListener(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
    public void SetIDPersonagem(int id)
    {
        idPersonagem = id;
    }
    public void SetImage(Sprite sprite)
    {
        Imagem.sprite = sprite;
    }

}
