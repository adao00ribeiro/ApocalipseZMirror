using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CriacaoPersonagem : MonoBehaviour, IPainelUpdate
{
    public GameObject Prefab;
    public Transform Container;
    private ScriptableData DataPersonagens
    {
        get
        {
            return GameObject.FindObjectOfType<ScriptableManager>().DataPersonagens;
        }
    }

    public Text TextCategoria;
    public Categorias categoria;
    public int indexCategoria = -1;
    public int indexItem = 0;
    public PersonalizaPersonagem personnagem
    {
        get
        {
            return GameObject.FindObjectOfType<PersonalizaPersonagem>();
        }
    }

    private void Start()
    {
        PreencherContainer();
    }

    public void PreencherContainer()
    {
        
        for (int i = 0; i < DataPersonagens.Count; i++)
        {

            SlotAvatarPersonagem slot = Instantiate(Prefab, Container).GetComponent<SlotAvatarPersonagem>();
            slot.Inicializa(i, DataPersonagens.GetSpriteAvatar(i), DataPersonagens.GetAp(i));
            if (i == 0)
            {
                slot.InstanciaPersonagem();
            }

        }
    }

    public void ProximaCategoria()
    {
        indexCategoria++;
        if (indexCategoria > sizeof(Categorias))
        {
            indexCategoria = 0;
        }
        categoria = (Categorias)indexCategoria;
        TextCategoria.text = categoria.ToString();
    }
    public void AnteriorCategoria()
    {
        indexCategoria--;
        if (indexCategoria < 0)
        {
            indexCategoria = sizeof(Categorias);
        }
        categoria = (Categorias)indexCategoria;
        TextCategoria.text = categoria.ToString();
    }
    public void ProximoItem()
    {
        indexItem++;
        if (indexItem > 3)
        {
            indexItem = 0;
        }
        personnagem.SetaItem(categoria, indexItem);
    }
    public void AnteriorItem()
    {
        indexItem--;
        if (indexItem < 0)
        {
            indexItem = sizeof(Categorias) - 1;
        }
        personnagem.SetaItem(categoria, indexItem);
    }
    public void CriarPersonagem()
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;
        // setainformacoes do personagem como id e itens setados na lista do jogador 

        if (ConnectionManager.testejogador.PersonagensCriados.Count < 6)
        {
            ConnectionManager.testejogador.PersonagensCriados.Add(spawPoint.GetChild(0).GetComponent<PersonalizaPersonagem>().GetInfoPersonagem());
            LobbyManager lobby =  GameObject.FindObjectOfType<LobbyManager>();
            lobby.ActivePainel(0);
        }
        else
        {
            print("Lista Cheia");
        }

    }

    public void Atualizar()
    {
        print("atualizando Criacao Personagem");
        ProximaCategoria();
      
    }

}
