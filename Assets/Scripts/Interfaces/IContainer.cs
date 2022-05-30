using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IContainer
{
    public event System.Action OnInventoryAltered;
    SSlotInventory GetSlotContainer ( int index);
    bool AddItem ( int slotIndex , SSlotInventory slot);
    bool AddItem ( SSlotInventory item  );
    void Clear ( );
    public bool CheckFreeSpace(ref int posicao);
    public bool CheckIfItemExist( int slotIndex );
    public void RemoveItem( SSlotInventory item );
    public void RemoveItem ( int slotIndex );
    public void UseItem( SSlotInventory item , bool closeInventory);
    public int GetMaxSlots ( );
    public void SetMaxSlots ( int maxslot);
    void MoveItem ( int id , int idmove );
    InventoryTemp GetContainerTemp ( );

    //COMMANDOS

    [Command]
    void CmdGetContainer ( NetworkConnectionToClient sender = null );
    [Command]
    void CmdMoveSlotContainer ( int idselecionado , int identer , NetworkConnectionToClient sender = null );
    [Command]
    void CmdAddSlotContainer ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
    [Command]
    void CmdRemoveSlotContainer ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
}
