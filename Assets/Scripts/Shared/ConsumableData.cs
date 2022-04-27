using System.Collections;
using DarkRift;
using UnityEngine;

[System.Serializable]
public class ConsumableData : ItemData
{
    [Header("CONSUMIVEL")]

    public int restauracao;
    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        restauracao = e.Reader.ReadInt32();
    }

    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(restauracao);
    }
}



