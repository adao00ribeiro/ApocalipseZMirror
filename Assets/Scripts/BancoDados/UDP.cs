using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class UDP 
{
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(BDClient.instance.ip), BDClient.instance.port);
        }

        /// <summary>Attempts to connect to the server via UDP.</summary>
        /// <param name="_localPort">The port number to bind the UDP socket to.</param>
        public void Connect()
        {
            socket = new UdpClient();
            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);
        try
        {
            string bemvindo = "OLA SOU CLIENTE UDP";
            byte[] me = Encoding.ASCII.GetBytes(bemvindo);
            SendData(me);  
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to server via UDP: {_ex}");
        }
    }

        /// <summary>Sends data to the client via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        public void SendData(Byte[] _packet)
        {
      
        try
            {
              
                if (socket != null)
                {

                     Console.WriteLine("enviando");
                    socket.BeginSend(_packet, _packet.Length, null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via UDP: {_ex}");
            }
        }

        /// <summary>Receives incoming UDP data.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                
            }
            catch
            {
                Disconnect();
            }
        }



        /// <summary>Disconnects from the server and cleans up the UDP connection.</summary>
        private void Disconnect()
        {
            BDClient.instance.Disconnect();
            endPoint = null;
            socket = null;
        }
    }
