using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
public class TCP 
{

    public TcpClient socket;
    private NetworkStream stream;
    private byte[] receivedData;
    private byte[] receiveBuffer;
    private int id;
    public TCP(int _id)
    {
        id = _id;
    }
    public void Connect()
    {
        socket = new TcpClient
        {
            ReceiveBufferSize = BDClient.instance.dataBufferSize,
            SendBufferSize = BDClient.instance.dataBufferSize
        };
        receiveBuffer = new byte[BDClient.instance.dataBufferSize];
        socket.BeginConnect(BDClient.instance.ip, BDClient.instance.port, ConnectCallback, socket);
    }

    private void ConnectCallback(IAsyncResult result)
    {
        socket.EndConnect(result);

        if (!socket.Connected)
        {
            Debug.Log("Sem Socket");
            return;
        }
        Debug.Log("Com Socket");
        stream = socket.GetStream();

        receivedData = new byte[BDClient.instance.dataBufferSize];

        stream.BeginRead(receiveBuffer, 0, BDClient.instance.dataBufferSize, ReceiveCallback, null);

        ClientSend.SendHelloServer(SENDMODE.TCP);
      
    }
    public void SendData(byte[] data)
    {
        try
        {
            if (socket != null)
            {
                stream.BeginWrite(data, 0, data.Length, null, null); // Send data to server
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error sending data to server via TCP: {ex}");
        }
    }
    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            int _byteLength = stream.EndRead(result);
            if (_byteLength <= 0)
            {
              BDClient.instance.Disconnect();
                return;
            }

            byte[] _data = new byte[_byteLength];
            Array.Copy(receiveBuffer, _data, _byteLength);

            ClientHandle.Handle(_data);
            //receiveData.Reset(HandleData(_data)); // Reset receivedData if all data was handled
            stream.BeginRead(receiveBuffer, 0, BDClient.instance.dataBufferSize, ReceiveCallback, null);
        }
        catch
        {
            Disconnect();
        }
    }

    private void Disconnect()
    {
        BDClient.instance.Disconnect();
        stream = null;
        receivedData = null;
        receiveBuffer = null;
        socket = null;
    }
}
