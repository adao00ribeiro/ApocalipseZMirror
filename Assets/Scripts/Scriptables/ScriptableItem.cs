using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCP_", menuName = "ScriptableObjects/Item", order = 1)]
public class ScriptableItem : ScriptableObject
{

    public string IDPREFAB = System.Guid.NewGuid().ToString();

   /// public ETypeItem Type;

    public string nameItem;

    public bool isStackable;

    public Sprite Thumbnail;

    public string Description;

    public int maxStacksize;

    public float Durability;

    public int QuantityDrop;

    public GameObject PrefabItem;


}

