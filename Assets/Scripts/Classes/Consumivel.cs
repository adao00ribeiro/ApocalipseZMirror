using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumivel : Item
{
   public ConsumableData data;
    public override INetworkingData GetData()
    {
        data.IdServe = IDSERVER;
        data.IdPrefab = ScriptableItem.IDPREFAB;
        data.Position = transform.position;
        data.Rotation = transform.rotation;

        return data; 
    }
    public override void SetData(INetworkingData _data)
    {
        data = (ConsumableData)_data;
    }
}
