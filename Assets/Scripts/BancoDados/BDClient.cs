using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
public enum BDTags
{

    //SERVIDOR
    S_BEMVINDO = 101,
    S_USUARIOREGISTRADO = 102,
    S_USUARIOCONSULTADO = 103,
    S_USUARIONAOENCONTRADO = 104,
    S_INFOUSUARIO = 105,

    //CLIENT
    C_REGISTRARUSUARIO = 202,
    C_CONSULTARUSUARIO = 203,
}
public class BDClient : MonoBehaviour
{
    public static BDClient instance;
    public byte[] receivedData;
    public int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public int port = 11000;

    public int myid = 0;

    public TCP tcp;
    public UDP udp;

    [SerializeField]public bool isConnected = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
     
        ConnectToServer();

        InvokeRepeating("Reconectar", 5,5);
    }
    public void Reconectar()
    {
       
        print("tentando a conexao com o banco de dados.........");
        if (isConnected == false)
        {
            ConnectToServer();
        }
    }
    public void teste()
    {

        LoginRequestData data = new LoginRequestData("General@hotmail.com", "SENHA000000");

        ClientSend.ConsultaUsuario(data, 1);
    }
    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        tcp = new TCP(myid);
        //  udp = new UDP();
        // InitializeClientData();
       
        tcp.Connect(); // Connect tcp, udp gets connected once tcp is done
       // udp.Connect();
    }

    public void SendDataTcp(byte[] data)
    {
        tcp.SendData(data);
    }
    public void SendDataUdp(byte[] data)
    {
        udp.SendData(data);
    }
    /// <summary>Disconnects from the server and stops all network traffic.</summary>
    public void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
          //  udp.socket.Close();
            Debug.Log("Disconnected from server.");
        }
    }
}