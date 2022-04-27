using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ETypeItem
{
    DEFAULT,
    BAG,
    WEAPON,
    CONSUMABLE,
    EQUIPPABLE

}
public abstract class Item : WorldObject , IItem, IInteracao
{
    
    [SerializeField] protected ScriptableItem ScriptableItem;
    [SerializeField] protected int Quantidade;

    public ScriptableItem GetScriptableItem()
    {
        return ScriptableItem;
    }
    public int GetQuantity()
    {
        return Quantidade;
    }
    public void SetQuantity(int quantity)
    {
        Quantidade = quantity;
    }
    public void Interacao(ServerPlayer caller)
    {

    }
    public void StartFocus()
    {
        
    }
    public virtual void EndFocus()
    {

    }


}
