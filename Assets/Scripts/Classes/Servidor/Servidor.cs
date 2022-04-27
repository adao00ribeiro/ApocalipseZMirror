using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;
using DarkRift.Server.Unity;
using System;

public enum RequestLogin
{
    ESPERANDO,
    NAOENCONTRADO,
    ENCONTRADO
}
[RequireComponent(typeof(XmlUnityServer))]
public class Servidor : MonoBehaviour
{
    public static Servidor Instance;
    private XmlUnityServer xmlServer;
    private DarkRiftServer server;
    private Timer timer;
    public Dictionary<ushort, ClientConnection> Connections = new Dictionary<ushort, ClientConnection>();
    public BDClient ClientBancoDados;
    private List<IEnumerator> ListaCoutine;
    public RequestLogin RequestLogin;
    public usuarioJogador infoUsuario;

    private void Awake()
    {
        timer = gameObject.AddComponent<Timer>();
        ListaCoutine = new List<IEnumerator>();
        Instance = this;
        DontDestroyOnLoad(this);
      
    }
    private void Start()
    {
        xmlServer = GetComponent<XmlUnityServer>();
        server = xmlServer.Server;


        server.ClientManager.ClientConnected += OnClientConnected;
        server.ClientManager.ClientDisconnected += OnClientDisconnected;
    }

    void OnDestroy()
    {
        server.ClientManager.ClientConnected -= OnClientConnected;
        server.ClientManager.ClientDisconnected -= OnClientDisconnected;
    }

    private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        IClient client = e.Client;
        ClientConnection p;
        if (Connections.TryGetValue(client.ID, out p))
        {
            p.OnClientDisconnect(sender, e);
        }
        else
        {
            e.Client.MessageReceived -= OnMessage;
        }
    }

    private void OnClientConnected(object sender, ClientConnectedEventArgs e)
    {
        e.Client.MessageReceived += OnMessage;
    }

    private void OnMessage(object sender, MessageReceivedEventArgs e)
    {
        IClient client = (IClient)sender;
        using (Message message = e.GetMessage())
        {
            switch ((Tags)message.Tag)
            {
                case Tags.LoginRequest:
                    OnclientLogin(client, message.Deserialize<LoginRequestData>());
                    break;
            }
        }
    }

    private void OnclientLogin(IClient client, LoginRequestData data)
    {
        
        bool Exist = false;
        foreach (ClientConnection  item in Connections.Values)
        {
            if (item.GetDadosUsuario().Name.Equals(data.Email))
            {
                Exist = true;
                break;
            }
        }
        //ConsultaosDadosnobancodedados
        ClientSend.ConsultaUsuario(data, client.ID);
        IEnumerator coroutine = EsperandoDados(client, data, Exist);
        ListaCoutine.Add(coroutine);
   
        StartCoroutine(coroutine);
       
    }

    IEnumerator EsperandoDados(IClient client, LoginRequestData data,bool Exist)
    {

        
         Debug.Log("esperando dados");
             yield return new WaitForSeconds(5);

           ClientConnection novaconexao = new ClientConnection(client, data);
            if (RequestLogin == RequestLogin.NAOENCONTRADO)
            {
            Debug.Log("CONEXAO REJEITADA");
            //envia que a conexao foi rejeitada
            novaconexao.EmiteMessageEmpty(Tags.LoginRequestDenied);
                novaconexao = null;
                yield return null;
              
            }
            if (RequestLogin == RequestLogin.ENCONTRADO)
            {
                novaconexao.Client.MessageReceived -= OnMessage;
                Connections.Add(client.ID, novaconexao);

                 InfoUsuarioData infoUsuarioData = infoUsuario.GetData();
                novaconexao.EmiteMessage(Tags.LoginRequestAccepted, infoUsuarioData, SendMode.Reliable);
                yield return null;
            }
        if (RequestLogin == RequestLogin.ESPERANDO)
        {
            Debug.Log("CONEXAO PERDIDA");
            //envia que a conexao foi rejeitada
            novaconexao.EmiteMessageEmpty(Tags.LoginRequestDenied);
            novaconexao = null;
            yield return null;
        }
    }
    public void ReceiveCallback(IAsyncResult ar)
    {

    }
}
