using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public interface IInventory
{
    public event System.Action OnInventoryAltered;
    SSlotInventory GetSlotInventory ( int index);
    bool AddItem ( SSlotInventory item  );
    void Clear ( );
    public bool CheckFreeSpace(ref int posicao);
    public bool CheckIfItemExist( SSlotInventory item );
    public void RemoveItem( SSlotInventory item , bool destroy);
    public void UseItem( SSlotInventory item , bool closeInventory);
    public int GetMaxSlots ( );
    public void SetMaxSlots ( int maxslot);
    void MoveItem ( int id , int idmove );
    InventoryTemp GetInventoryTemp ( );

    //COMMANDOS

    [Command]
    void CmdGetInventory ( NetworkConnectionToClient sender = null );
    [Command]
    void CmdMoveSlotInventory ( int idselecionado , int identer , NetworkConnectionToClient sender = null );
    [Command]
    void CmdAddSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
    [Command]
    void CmdRemoveSlotInventory ( UISlotItemTemp slot , NetworkConnectionToClient sender = null );
}
