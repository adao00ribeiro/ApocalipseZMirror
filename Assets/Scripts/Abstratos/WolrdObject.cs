using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class WorldObject : MonoBehaviour,IWorldObject
{
    [SerializeField] protected string IDSERVER;
    [SerializeField] protected string IDPREFAB;

    public abstract INetworkingData GetData();
    public abstract void SetData(INetworkingData data);
   
    public void SetIDSERVER(string ID)
    {
        IDSERVER = ID;
    }

    public void SetIDPREFAB(string ID)
    {
        IDPREFAB = ID;
    }


}
