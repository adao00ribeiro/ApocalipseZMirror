using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public static class TargetFpsPlayer
{
    [TargetRpc]
    public static void TargetGetContainer ( TypeContainer type , NetworkConnection target , InventoryTemp inventory )
    {
        target.identity.GetComponent<>

        SetMaxSlots ( inventory.maxSlot );
        Clear ( );
        for ( int i = 0 ; i < inventory.slot.Count ; i++ )
        {
            ScriptableItem item = ScriptableManager.GetScriptable(inventory .slot[i].guidid);
            if ( item != null )
            {
                AddItem ( inventory.slot[i].slotindex , new SSlotInventory ( inventory.slot[i].slotindex , item.sitem , inventory.slot[i].Quantity ) );
            }

        }

    }
    public static IContainer GetContainerCommandd ( TypeContainer type , FpsPlayer player )
    {
        IContainer tempContainer = null;
        switch ( type )
        {
            case TypeContainer.INVENTORY:
                tempContainer =  player.GetInventory ( );
                break;
            case TypeContainer.FASTITEMS:
                tempContainer =  player.GetFastItems ( );
                break;
            case TypeContainer.WEAPONS:
                
                break;
        }

        return tempContainer;
    }
}