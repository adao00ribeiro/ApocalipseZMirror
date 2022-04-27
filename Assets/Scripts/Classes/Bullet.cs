using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Item
{
    public BulletData data;

    private void Update()
    {
       
    }
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
        data = (BulletData)_data;
    }
}
