using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
using System;

public struct SlotInventoryTemp
{
    public int slotindex;
    public string guidid;
    public int Quantity;
    public SlotInventoryTemp ( int _slotindex, string _guidid , int _Quantity )
    {
        slotindex = _slotindex;
        guidid = _guidid;
        Quantity = _Quantity;
    }
    
}
public static class SlotInventoryReadWrite
{
    public static void WriteStringTest ( this NetworkWriter writer , SlotInventoryTemp value )
    {
        writer.WriteInt ( value.slotindex );
        writer.WriteString ( value.guidid );
        writer.WriteInt ( value.Quantity );
    }
    public static SlotInventoryTemp ReadStringTest ( this NetworkReader reader )
    {

        return new SlotInventoryTemp ( reader .ReadInt() , reader.ReadString ( ) , reader.ReadInt ( ) );

    }
}
