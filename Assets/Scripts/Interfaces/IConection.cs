using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System;

public interface IConection
{
    void Conectar();
    void SetOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage);
    void RetirarOnMensagem(EventHandler<MessageReceivedEventArgs> OnMensage);

    UnityClient GetUnityClient();
    void Destroy();
    string StatuServer();

    void SetJogador(usuarioJogador jogador);
    usuarioJogador GetJogador();
}
