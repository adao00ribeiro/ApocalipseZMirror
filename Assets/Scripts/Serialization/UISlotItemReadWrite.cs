using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public struct UISlotItemTemp
{
    public int id;
    public SSlotInventory slot;

    public UISlotItemTemp ( int _id , SSlotInventory _slot )
    {
        id = _id;
        slot = _slot;
    }
}
public static class UISlotItemReadWrite 
{
    public static void WriteStringTest ( this NetworkWriter writer , UISlotItemTemp value )
    {
      
        writer.WriteInt ( value.id );
        writer.Write ( value.slot );
    }
    public static UISlotItemTemp ReadStringTest ( this NetworkReader reader )
    {
        return new UISlotItemTemp ( reader.ReadInt ( ) , reader.Read<SSlotInventory> ( ) );
    }
}