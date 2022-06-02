using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
using System;

public static class CommandsFpsPlayer
{
    #region COMMAND
    [Command]
    public static void CmdGetContainer ( TypeContainer type , NetworkConnectionToClient sender = null )
    {
        NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
        IContainer tempContainer = TargetFpsPlayer.GetContainerCommandd(type,sender.identity.GetComponent<FpsPlayer> ( ));
        TargetFpsPlayer.TargetGetContainer ( type , opponentIdentity.connectionToClient , tempContainer.GetContainerTemp ( ) );
    }
    [Command]
    public static void CmdMoveSlotContainer ( TypeContainer type , int idselecionado , int identer , NetworkConnectionToClient sender = null )
    {
        NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
        IContainer tempContainer = TargetFpsPlayer.GetContainerCommandd(type,sender.identity.GetComponent<FpsPlayer> ( ));

        tempContainer.MoveItem ( idselecionado, identer );

        TargetFpsPlayer.TargetGetContainer ( type , opponentIdentity.connectionToClient , tempContainer.GetContainerTemp ( ) );
    }
    [Command]
    public static void CmdAddSlotContainer ( TypeContainer type , UISlotItemTemp slot , NetworkConnectionToClient sender = null )
    {
        NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
        IContainer tempContainer = TargetFpsPlayer.GetContainerCommandd(type,sender.identity.GetComponent<FpsPlayer> ( ));

        ScriptableItem item = ScriptableManager.GetScriptable(slot.slot.guidid) ;
        SSlotInventory slotnovo;

        if ( item )
        {

            slotnovo = new SSlotInventory ( slot.slotIndex , item.sitem , slot.slot.Quantity );

            tempContainer.AddItem ( slot.slotIndex , slotnovo );

        }

       
        TargetFpsPlayer.TargetGetContainer ( type , opponentIdentity.connectionToClient , tempContainer.GetContainerTemp ( ) );

    }
    [Command]
    public static void CmdRemoveSlotContainer ( TypeContainer type , UISlotItemTemp slot , NetworkConnectionToClient sender = null )
    {
       
        NetworkIdentity opponentIdentity = sender.identity.GetComponent<NetworkIdentity>();
        IContainer tempContainer = TargetFpsPlayer.GetContainerCommandd(type,sender.identity.GetComponent<FpsPlayer> ( ));
        tempContainer.RemoveItem ( slot.slot.slotindex );


        TargetFpsPlayer.TargetGetContainer ( type , opponentIdentity.connectionToClient , tempContainer.GetContainerTemp ( ) );
    }

    internal static void CmdFire ( NetworkConnectionToClient sender = null )
    {

        GameObject projectile = Instantiate(projectilePrefab, projectileMount.position, projectileMount.rotation);
        NetworkServer.Spawn ( projectile );
    }


    #endregion
}
