using DarkRift;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ObjectData
{
    [Header("ITEM")]
    public int Quantidade;
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        Quantidade = e.Reader.ReadInt32();
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(Quantidade);
    }
}
