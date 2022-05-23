using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public static class SlotInventoryReadWrite 
{
        public static void WriteStringTest ( this NetworkWriter writer , SSlotInventory value )
        {
        if ( value.ItemIsNull())
        {
            writer.WriteString ( "-1" );
            writer.WriteInt ( 0);
            return;
        }
        writer.WriteString ( value.GetSItem ( ).GuidId );
        writer.WriteInt ( value.GetQuantity ( ) );
    }
        public static SSlotInventory ReadStringTest ( this NetworkReader reader )
        {
         ScriptableItem scriptableitem = ScriptableManager.GetScriptable ( reader.ReadString());

        if (scriptableitem )
        {
            return new SSlotInventory ( scriptableitem.sitem , reader.ReadInt ( ) );
        }

        return new SSlotInventory ( null, reader.ReadInt ( ) );


    }
    }