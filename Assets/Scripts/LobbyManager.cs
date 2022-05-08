using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    [Header("PAINEL")]
    public Transform MIDDLE;
    public int testePainel;
    public LobbyInfoData data;
    private void Awake()
    {
        StructUser structuser = new StructUser();
        structuser.id = 1;
        structuser.username = "adao00ribeiro";
        structuser.password = "123456";
        structuser.email = "adao-eduardo@hotmail.com";
        structuser.maxslot = 24;
        structuser.dp = 5000;
        structuser.ap = 1214;

        GameObject.FindObjectOfType<InformacaoClient>().userdata = new User(structuser);

        //testedepersonagencriados
        InfoPersonagem yasmin = new InfoPersonagem();
        yasmin.teste();
        GameObject.FindObjectOfType<InformacaoClient>().userdata.PersonagensCriados = new List<InfoPersonagem>(6);
        GameObject.FindObjectOfType<InformacaoClient>().userdata.PersonagensCriados.Add(yasmin);

        ActivePainel(0);

        RoomData[] romms = new RoomData[2];
        romms[0] = new RoomData("SERVIDOR 1 ", 15, 65);
        romms[1] = new RoomData("SERVIDOR 2 ", 2, 65);
        data = new LobbyInfoData(romms);

        InstanciaPersonagem(5);
    }
    // Update is called once per frame
    void Update()
    {
    }



    /*
     *    // Start is called before the first frame update
    void Start()
    {
        ConnectionManager.Instance.Client.MessageReceived += OnMessage;
        using (Message message = Message.CreateEmpty((ushort)Tags.LobbyGetRoom))
        {
            ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void OnDestroy()
    {
        ConnectionManager.Instance.Client.MessageReceived -= OnMessage;
    }
    private void OnMessage(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        {
            switch ((Tags)message.Tag)
            {
                case Tags.LobbyJoinRoomDenied:
                    OnRoomJoinDenied(message.Deserialize<LobbyInfoData>());
                    break;
                case Tags.LobbyGetRoom:
                    AtualizaRooms(message.Deserialize<LoginInfoData>());
                    break;
                case Tags.LobbyJoinRoomAccepted:
                    //sucesso ao entrar na sala
                    OnRoomJoinAcepted();
                    break;
            }
        }
    }
    //acoplado ao bottao do lobby na escolha das salas
    public void SendJoinRoomRequest(string roomName)
    {
        using (Message message = Message.Create((ushort)Tags.LobbyJoinRoomRequest, new JoinRoomRequest(roomName)))
        {
            ConnectionManager.Instance.Client.SendMessage(message, SendMode.Reliable);
        }
    }
    public void AtualizaRooms(LoginInfoData data)
    {
        ConnectionManager.Instance.Account = new Conta(data.Id, "PLAYER");
        ConnectionManager.Instance.LobbyInfoData = data.Data;
        RefreshRooms(ConnectionManager.Instance.LobbyInfoData);
    }
    public void OnRoomJoinDenied(LobbyInfoData data)
    {
        RefreshRooms(data);
    }
    //muda para nova cena de jogo
    public void OnRoomJoinAcepted()
    {
        ConnectionManager.Instance.SceneController.LoadSceneAsync("Game");
    }
    public void RefreshRooms(LobbyInfoData data)
    {
        RoomListObject[] roomObjects = roomListContainerTransform.GetComponentsInChildren<RoomListObject>();
        if (roomObjects.Length > data.Rooms.Length)
        {
            for (int i = data.Rooms.Length; i < roomObjects.Length; i++)
            {
                Destroy(roomObjects[i].gameObject);
            }
        }
        for (int i = 0; i < data.Rooms.Length; i++)
        {
            RoomData d = data.Rooms[i];
            if (i < roomObjects.Length)
            {
                roomObjects[i].Set(this, d);
            }
            else
            {
                GameObject go = Instantiate(roomListPrefab, roomListContainerTransform);
                go.GetComponent<RoomListObject>().Set(this, d);
            }
        }
    }
    */

    public void ActivePainel(int IndexPainel)
    {

        foreach (Transform item in MIDDLE)
        {
            item.gameObject.SetActive(false);
        }
        IPainelUpdate painel = MIDDLE.GetChild(IndexPainel).GetComponent<IPainelUpdate>();
        MIDDLE.GetChild(IndexPainel).gameObject.SetActive(true);
        painel.Atualizar();
        if (IndexPainel != 0)
        {
            MIDDLE.GetChild(5).gameObject.SetActive(true);
        }

    }

    public void InstanciaPersonagem(int idPersonagem)
    {
        Transform spawPoint = GameObject.Find("SpawPersonagem").transform;
        if (spawPoint.childCount > 0)
        {
            Destroy(spawPoint.GetChild(0).gameObject);
        }

        GameObject.FindObjectOfType<ScriptableManager>().InstanciaPersonagem(idPersonagem, spawPoint);

    }
}