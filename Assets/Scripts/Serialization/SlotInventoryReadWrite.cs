using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
using System;

public struct SlotInventoryTemp
{
    public string guidid;
    public int Quantity;
    public SlotInventoryTemp ( string _guidid , int _Quantity )
    {
        guidid = _guidid;
        Quantity = _Quantity;
    }
    
}
public static class SlotInventoryReadWrite
{
    public static void WriteStringTest ( this NetworkWriter writer , SlotInventoryTemp value )
    {
        writer.WriteString ( value.guidid );
        writer.WriteInt ( value.Quantity );
    }
    public static SlotInventoryTemp ReadStringTest ( this NetworkReader reader )
    {

        return new SlotInventoryTemp ( reader.ReadString ( ) , reader.ReadInt ( ) );

    }
}
