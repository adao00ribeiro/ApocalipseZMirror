using System.Collections;
using DarkRift;
using UnityEngine;
public struct WeaponData : IDarkRiftSerializable
{
  //  public ItemData itemData;
    public void Deserialize(DeserializeEvent e)
    {
     //   itemData = e.Reader.ReadSerializable<ItemData>();
    }

    public void Serialize(SerializeEvent e)
    {
    //    e.Writer.Write(itemData);
    }
}