using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public static class ItemSerializer
{
    public static void WriteItem ( this NetworkWriter writer , SItem item )
    {
        writer.WriteString ( item.name);
    }
}