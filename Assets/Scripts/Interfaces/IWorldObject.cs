using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldObject 
{
     INetworkingData GetData();
    void SetData(INetworkingData data);
     void SetIDSERVER(string ID);
     void SetIDPREFAB(string ID);
}
