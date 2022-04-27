using DarkRift;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUsuarioData : INetworkingData
{
    public int ID;
    public string UserName;
    public int DP;
    public int AP;
    public string Email;
    public string Senha;

    public void Deserialize(DeserializeEvent e)
    {
        ID = e.Reader.ReadInt32();
        UserName = e.Reader.ReadString();
        DP = e.Reader.ReadInt32();
        AP = e.Reader.ReadInt32();
        Email = e.Reader.ReadString();
        Senha = e.Reader.ReadString();

    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(ID);
        e.Writer.Write(UserName);
        e.Writer.Write(DP);
        e.Writer.Write(AP);
        e.Writer.Write(Email);
        e.Writer.Write(Senha);
    }
}
