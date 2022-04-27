using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem 
{
    int GetQuantity();
    void SetQuantity(int quantity);
    ScriptableItem GetScriptableItem();
}
