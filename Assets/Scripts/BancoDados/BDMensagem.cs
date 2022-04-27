using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BDMensagem : IDisposable
{
    public int ID;
    public ushort Tag;
    public string Json;
    public int IdGameClient;
    public static BDMensagem Create(int idclient, ushort tag, string json)
    {
        BDMensagem message = new BDMensagem();
        message.ID = idclient;
        message.Tag = tag;
        message.Json = json;

        return message;
    }
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
