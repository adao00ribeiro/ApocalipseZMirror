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
        IContainer container = GetContainerCommandd( type,target.identity.GetComponent<FpsPlayer>());

        container.SetMaxSlots ( inventory.maxSlot );
        container.Clear ( );
        for ( int i = 0 ; i < inventory.slot.Count ; i++ )
        {
            ScriptableItem item = ScriptableManager.GetScriptable(inventory .slot[i].guidid);
            if ( item != null )
            {
                container.AddItem ( inventory.slot[i].slotindex , new SSlotInventory ( inventory.slot[i].slotindex , item.sitem , inventory.slot[i].Quantity ) );
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
                tempContainer = player.GetWeaponsSlots ( );
                break;
        }

        return tempContainer;
    }
}