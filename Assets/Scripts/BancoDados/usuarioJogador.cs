using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class usuarioJogador
{
    public int ID;
    public string UserName;
    public int DP;
    public int AP;
    public string Email;
    public string Senha;

    //estrutura aki , int nao
    public List<InfoPersonagem> PersonagensCriados  = new List<InfoPersonagem>(6);
    public InfoPersonagem personagemSelecionado;
    public List<int> ListIDCharacterComprados = new List<int>();
    public List<int> ListIDSkinsComprados = new List<int>();

    public usuarioJogador( InfoUsuarioData data)
    {
        ID = data.ID;
        UserName = data.UserName;
        DP = data.DP;
        AP = data.AP;
        Email = data.Email;
        Senha = data.Senha;
    }

    public usuarioJogador(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    internal InfoUsuarioData GetData()
    {
        InfoUsuarioData data = new InfoUsuarioData();

       data.ID =        ID;
       data.UserName =  UserName;
       data.DP =        DP;
       data.AP =        AP;
       data.Email =     Email;
        data. Senha =    Senha;

        return data;
    }
}
