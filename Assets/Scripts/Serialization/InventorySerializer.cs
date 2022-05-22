using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public static class InventorySerializer
{
  
    public static void WriteItem ( this NetworkWriter writer , SSlotInventory item )
    {
        if ( item is Weapon weapon )
        {
            writer.WriteByte ( WEAPON );
            writer.WriteString ( weapon.name );
            writer.WritePackedInt32 ( weapon.hitPoints );
        }
        else if ( item is Armor armor )
        {
            writer.WriteByte ( ARMOR );
            writer.WriteString ( armor.name );
            writer.WritePackedInt32 ( armor.hitPoints );
            writer.WritePackedInt32 ( armor.level );
        }
    }
}