using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;
using System.Text;
public class ClientHandle 
{

    internal static void Handle(byte[] data)
    {
        string text = Encoding.UTF8.GetString(data);
        using (BDMensagem mensagem = JsonUtility.FromJson<BDMensagem>(text))
        {
            switch (mensagem.Tag)
            {
                case (ushort)BDTags.S_BEMVINDO:
                    BDClient.instance.isConnected = true;
                    BDClient.instance.myid = mensagem.ID;
                    Debug.Log(mensagem.Json);

                break;
                case (ushort)BDTags.S_USUARIONAOENCONTRADO:
                    Debug.Log(mensagem.Json);
                    Servidor.Instance.RequestLogin = RequestLogin.NAOENCONTRADO;
                    break;
                case (ushort)BDTags.S_INFOUSUARIO:
                    Debug.Log(mensagem.Json);
                    Servidor.Instance.RequestLogin = RequestLogin.ENCONTRADO;
                    Servidor.Instance.infoUsuario = Empacotamento.Desempacotar<usuarioJogador>(mensagem.Json);

                    break;

            }
        }
    }

    
}
