using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net;
using System;

public enum SENDMODE
{
    TCP,
    UDP
}
public class ClientSend 
{

    private static void Send( byte[] data, SENDMODE mode)
    {

        if (mode == SENDMODE.TCP)
        {
            BDClient.instance.tcp.SendData(data);

            return;
        }

        BDClient.instance.udp.SendData(data);
    }

    public static void SendHelloServer(SENDMODE tcp)
    {
        using (BDMensagem mensagem = BDMensagem.Create(BDClient.instance.myid, (ushort)BDTags.S_BEMVINDO, "OLA EU SOU CLIENT DARK RIFT"))
        {

            Send(Empacotamento.Empacotar(mensagem), tcp);
        }
    }

    public static void ConsultaUsuario(LoginRequestData data ,int idClientGame)
    {
     
        usuarioJogador user = new usuarioJogador(data.Email, data.Senha);
        
        using (BDMensagem mensagem = BDMensagem.Create(BDClient.instance.myid, (ushort)BDTags.C_CONSULTARUSUARIO, Empacotamento.EmpacotarParaString(user)))
        {
            mensagem.IdGameClient = idClientGame;

            Send(Empacotamento.Empacotar(mensagem), SENDMODE.TCP);
           
        }


    }
    public static void RegistrarUsuario(int IDSERVIDOR, int idClient)
    {

    }
}