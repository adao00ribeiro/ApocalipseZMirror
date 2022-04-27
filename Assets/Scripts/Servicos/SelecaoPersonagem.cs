using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecaoPersonagem : MonoBehaviour,IPainelUpdate
{
    public GameObject PrefabAvatarSelecionado;
    public Sprite spriteMais;
    public Transform Container;
    [SerializeField] private LobbyManager lobby;
    [SerializeField] private ScriptableManager scriptableManager;

    InformacaoClient infoclient;
    // Start is called before the first frame update
    private void Awake()
    {
        lobby = GameObject.FindObjectOfType<LobbyManager>();
        infoclient = GameObject.FindObjectOfType<InformacaoClient> ( );
        scriptableManager = GameObject.FindObjectOfType<ScriptableManager>();
        InstanciaSlots();

        
    }


    public void Jogar()
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;
        infoclient.userdata.personagemSelecionado = spawPoint.GetChild(0).GetComponent<PersonalizaPersonagem>().GetInfoPersonagem();

        // ativarpainel de servidores

    }
   

    public void InstanciaSlots()
    {
        for (int i = 0; i < 6; i++)
        {
            SlotAvatarSelecionado slot = Instantiate(PrefabAvatarSelecionado, Container).GetComponent<SlotAvatarSelecionado>();
         
        }
       
    }
    public void AtualizaSlots()
    {
       for (int i = 0; i < 6; i++)
        {
            SlotAvatarSelecionado slot = Container.GetChild(i).GetComponent<SlotAvatarSelecionado>();
            slot.SetImage(spriteMais);
            slot.AddListener(() => lobby.ActivePainel(2));
        }
        for (int i = 0; i < infoclient.userdata.PersonagensCriados.Count; i++)
        {
            InfoPersonagem info =  infoclient.userdata.PersonagensCriados[i];
            SlotAvatarSelecionado slot = Container.GetChild(i).GetComponent<SlotAvatarSelecionado>();
            slot.SetImage(scriptableManager.DataPersonagens.GetSpriteAvatar(info.IDPrefab));

            slot.AddListener(() => {
                infoclient.userdata.personagemSelecionado = info;
                lobby.InstanciaPersonagem(info.IDPrefab);
            });
        }
    }

    public void Atualizar()
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;

        if (spawPoint.childCount > 0)
        {
            Destroy(spawPoint.GetChild(0).gameObject);
        }

        AtualizaSlots();
    }
    public void Deletar()
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;
        ConnectionManager.testejogador.PersonagensCriados.Remove(ConnectionManager.testejogador.personagemSelecionado);
      
        Atualizar();
        AtualizaSlots();


    }
}
