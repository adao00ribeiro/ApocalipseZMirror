using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Threading;

public class testebd : MonoBehaviour
{
    // read Thread
    Thread readThread;
    UdpClient serve;
    
   
    void Start()
    {

        UDPTask("127.0.0.1", 11000, Encoding.ASCII.GetBytes("testeeeeee"));

    }
    private void Update()
    {
       
        if (Input.GetKeyDown("q"))
            stopThread();
    }
    private void stopThread()
    {
        if (readThread.IsAlive)
        {
            readThread.Abort();
        }
        serve.Close();
    }
    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                // receive bytes
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = serve.Receive(ref anyIP);

                // encode UTF8-coded bytes to text format
                string text = Encoding.UTF8.GetString(data);
                // show received message
                print(">> " + text);

                if (data.Length>0)
                {
                    stopThread();
                }

            }
            catch (System.Exception err)
            {
                print(err.ToString());
            }
        }
    }
    public  void UDPTask(string IpDest, int Port, byte[] buffer)
    {
        /*

        //funcionando
        serve = new UdpClient();
        serve.Client.Blocking = false;
        IPEndPoint hostEP = new IPEndPoint(IPAddress.Parse(IpDest), Port);
        serve.Connect(hostEP);

        //Cria a chamada Socket

        string json = JsonUtility.ToJson(BDMensagem.Create(1,"olamundo"));

        byte[] buffers = Encoding.ASCII.GetBytes(json);

        //Envia os dados em byteArray.
        //serve.SendTo(buffers, ep);
        serve.Send(buffers, buffers.Length);
        //tempo limite para receber
        serve.Client.ReceiveTimeout = 1000;
        readThread = new Thread(new ThreadStart(ReceiveData));
         readThread.IsBackground = true;
         readThread.Start();

        */
    }

}
