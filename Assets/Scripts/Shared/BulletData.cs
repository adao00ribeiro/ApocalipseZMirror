using System.Collections;
using DarkRift;
using UnityEngine;

[System.Serializable]
public class BulletData : ItemData
{
    [Header("BULLET")]
    public int dano;

    public override void Deserialize(DeserializeEvent e)
    {
        base.Deserialize(e);
        dano = e.Reader.ReadInt32();
    }
    public override void Serialize(SerializeEvent e)
    {
        base.Serialize(e);
        e.Writer.Write(dano);
    }
}