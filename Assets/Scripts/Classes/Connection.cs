using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(UnityClient))]
public class Connection :MonoBehaviour,IConection
{
    public string IPAddress;
    public int port;
    private UnityClient Client;

    private usuarioJogador InformacaoJogador;

    private void Awake()
    {
        Client = GetComponent<UnityClient>();
        ConnectionManager.SetIConection(this);
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
       
      //  Pgameobject.AddComponent<SceneController>();
      //  Pgameobject.AddComponent<ScTimer>();
      //  Pgameobject.AddComponent<ScSubmit>();
      //  Pgameobject.AddComponent<ScInputController>();
    
     //   Invoke("teste",4);
     
    }
    public string StatuServer()
    {
        if (Client.ConnectionState == ConnectionState.Connected)
        {
            return "Servidor Connectado";
        }
        if (Client.ConnectionState == ConnectionState.Disconnected)
        {
            //verifica a cena se nao tiver na main muda para main
            if (false)
            {

            }
            //volta para o menu
            ConnectionManager.Conectar();

            return "Servidor Offline";
        }
        if (Client.ConnectionState == ConnectionState.Connecting)
        {

            return "Connectando....";
        }

        return "Servidor Online";
    }

    public void Conectar()
    {
        Client.ConnectInBackground(IPAddress, port, false, ConnectCallback);
    }
    private void ConnectCallback(Exception exception)
    {
        if (Client.ConnectionState == ConnectionState.Connected)
        {
            Debug.Log("Connectado");
          
        }
        else
        {
            Debug.LogError("Unable to connect to server.");
        }
    }

    //recebe a mensagem 
    public void SetOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage)
    {
        Client.MessageReceived += OnMensage;
    }
    public void RetirarOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage)
    {
       Client.MessageReceived -= OnMensage;
    }

    public void SetJogador(usuarioJogador jogador  )
    {
        InformacaoJogador = jogador;
    }
    public UnityClient GetUnityClient()
    {
        return Client;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public usuarioJogador GetJogador()
    {
        return InformacaoJogador;
    }
}
