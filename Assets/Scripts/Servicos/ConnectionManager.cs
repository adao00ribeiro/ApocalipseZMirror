using System;
using System.Net;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.UI;
public enum StateServer
{
    CONNECTED,
    DISCONNECTED,
    INTERRUPT
}
public static class ConnectionManager
{
    private static IConection conn;
	
    public static usuarioJogador testejogador = new usuarioJogador("ADAO-EDUARDO@HOTMAIL.COM", "123456");
    public static void SetIConection(IConection _conn)
    {
        conn = _conn;
    }
   public static void Conectar()
    {
        conn.Conectar();
    }
    public static string TextStatuServe()
    {
        return conn.StatuServer();
    }
    public static void SetOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage)
    {
        conn.SetOnMensagem(OnMensage);
    }
    public static void RetirarOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage)
    {
        conn.RetirarOnMensagem(OnMensage);
    }
    public static void EmiteMessage(Tags tag, INetworkingData data, SendMode sendmode)
    {
        try
        {
            using (Message m = Message.Create((ushort)tag, data))
            {
                UnityClient client = conn.GetUnityClient();
                client.SendMessage(m, sendmode);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
       
    }

    public static void DestroyConnection()
    {
        conn.Destroy();
    }
    public static void EmiteMessageEmpty(Tags tag)
    {
        using (Message m = Message.CreateEmpty((ushort)tag))
        {
            UnityClient client = conn.GetUnityClient();
            client.SendMessage(m, SendMode.Reliable);
        }
    }

    public static void SetJogador(usuarioJogador jogador)
    {
        if (jogador !=null)
        {
            conn.SetJogador(jogador);
            Debug.Log("JOGADOR SETADO");
            return;
        }
        Debug.Log("JOGADOR nulll");
    }
    public static usuarioJogador GetJogador()
    {
        return conn.GetJogador();
    }
}
