using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;

public struct InventoryTemp
{
    public List<SSlotInventory> slot;
    public int maxSlot;

    public InventoryTemp ( List<SSlotInventory> _slot , int maxslot )
    {
        slot = _slot;
        maxSlot = maxslot;
    }
}
public static class InventoryReadWrite
{
    public static void WriteInventory( this NetworkWriter writer , InventoryTemp value )
    {
       
        writer.WriteList ( value.slot ) ;
        writer.WriteInt ( value.maxSlot );

    }
    public static InventoryTemp ReadInventory ( this NetworkReader reader )
    {
       
        return new InventoryTemp( reader.ReadList<SSlotInventory> ( ) , reader.ReadInt())  ;

    }
}