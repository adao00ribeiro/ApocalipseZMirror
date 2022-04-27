using System.Collections;
using System.Collections.Generic;
using DarkRift;

public struct InventoryData : IDarkRiftSerializable
{
    public ushort Id;
    public WeaponData[] WeaponItens;
    public ConsumableData[] ConsumableItens;

    public void Deserialize(DeserializeEvent e)
    {
        WeaponItens = e.Reader.ReadSerializables<WeaponData>();
        ConsumableItens = e.Reader.ReadSerializables<ConsumableData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(WeaponItens);
        e.Writer.Write(ConsumableItens);
    }
}
