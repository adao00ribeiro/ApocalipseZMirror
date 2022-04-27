using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour , IRoom
{
    private Scene scene;

    private Dictionary<ushort, ClientConnection> ClientConnections = new Dictionary<ushort, ClientConnection>();

    [SerializeField] private List<ServerPlayer> serverPlayers = new List<ServerPlayer>();
    public List<ushort> DespawPlayers = new List<ushort>(4);


    [SerializeField] private Dictionary<string, IItem> ItemStatic = new Dictionary<string, IItem>();
    public List<Item> ITENSVISU = new List<Item>();
    public List<string> DespawItens = new List<string>(4);

    //public static ScTimer Timer;

    //    private readonly IClienteRepository _clienteRepository;
    [Header("Public Fields")]
	[SerializeField]private string Name;
    [SerializeField]private byte MaxSlots;

	[Header("Prefabs")]
    [SerializeField] private ScriptableData ListPrefabCharacter;
    [SerializeField] private ScriptableData ListPrefabItens;

    private void Awake()
    {
       IItem[] TempItensMap = GameObject.FindObjectsOfType<Item>();
        foreach (var item in TempItensMap)
        {
            ItemStatic.Add(System.Guid.NewGuid().ToString(), item);
            ITENSVISU.Add((Item)item);
        }
      
    }

    public void AddPlayerToRoom(ClientConnection conn)
    {
        ClientConnections.Add(conn.Client.ID, conn);
        conn.SetRoom(this);
        conn.EmiteMessageEmpty(Tags.LobbyJoinRoomAccepted);
    }

    public void Initialize(string name, byte maxSlots)
    {
        Name = name;
        MaxSlots = maxSlots;
        scene = SceneManager.CreateScene("Room_" + name);
        SceneManager.MoveGameObjectToScene(gameObject, scene);
    }

    public void JoinPlayerToGame(ClientConnection conn)
    {
        ServerPlayer player = Instantiate(ListPrefabCharacter.GetGameObject(conn.GetDadosUsuario().IdPrefabCharacter), Vector3.zero, Quaternion.identity, this.transform).AddComponent<ServerPlayer>();
        //Destroy(player.GetComponent<ClientPlayer>());
        serverPlayers.Add(player);
       // player.Initialize(Vector3.zero, conn);

      // conn.EmiteMessage(Tags.GameStartDataResponse,EMPACOTAMENTO());
   
    }

    public void RemovePlayerToRoom(ClientConnection coon)
    {
        DespawPlayers.Add(coon.Client.ID);
        Destroy(coon.Player.gameObject);
        ClientConnections.Remove(coon.Client.ID);
        serverPlayers.Remove(coon.Player);
        coon.SetRoom(null);
    }

    public void Close()
    {
        foreach (ClientConnection p in ClientConnections.Values)
        {
            RemovePlayerToRoom(p);
        }
        Destroy(gameObject);
    }

    public int GetMaxConnections()
    {
        return ClientConnections.Count;
    }

    public string GetName()
    {
        return Name;
    }

    public byte GetMaxSlots()
    {
        return MaxSlots;
    }
}
