using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IContainer
{
    public event System.Action OnContainerAltered;
    SSlotInventory GetSlotContainer ( int index);
    bool AddItem ( int slotIndex , SSlotInventory slot);
    bool AddItem ( SSlotInventory item  );
    void Clear ( );
    public bool CheckFreeSpace(ref int posicao);
    public bool CheckIfItemExist( int slotIndex );
    public void RemoveItem ( int slotIndex );
    public void UseItem( SSlotInventory item , bool closeInventory);
    public int GetMaxSlots ( );
    public void SetMaxSlots ( int maxslot);
    void MoveItem ( int id , int idmove );
    InventoryTemp GetContainerTemp ( );
    void InvokeOnContainer ( );
    TypeContainer GetTypeContainer ( );
    void Update ( SlotInventoryTemp slot );
    #region COMMAND
    [Command]
    void CmdGetContainer ( TypeContainer type , NetworkConnectionToClient sender = null );
    [Command]
    void CmdMoveSlotContainer ( TypeContainer type , int idselecionado , int identer , NetworkConnectionToClient sender = null );
    [Command]
    void CmdAddSlotContainer ( TypeContainer type , UISlotItemTemp slot , NetworkConnectionToClient sender = null );
    [Command]
    void CmdRemoveSlotContainer ( TypeContainer type , UISlotItemTemp slot , NetworkConnectionToClient sender = null );
    [Command]
    void CmdUpdateSlot ( TypeContainer type , SlotInventoryTemp slot , NetworkConnectionToClient sender = null );
    #endregion

    #region TARGET
    [TargetRpc]
    void TargetGetContainer ( NetworkConnection target ,TypeContainer type , InventoryTemp inventory );
   
    #endregion
}
