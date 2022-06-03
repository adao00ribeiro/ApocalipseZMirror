using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using ApocalipseZ;
public struct SpawObjectTransform
{
    public string guidid;
    public Vector3 position;
    public Quaternion rotation;
}
public struct ConnectMessage : NetworkMessage
{
    public int score;
    public Vector3 scorePos;
    public int lives;
}
public class SpawObjects : NetworkBehaviour
{
    public GameObject treePrefab;
    private void Awake ( )
    {
        ScriptableItem[] weapons =  ScriptableManager.GetItemsWeapons ( );
        for ( int i = 0 ; i < weapons.Length ; i++ )
        {
            NetworkClient.RegisterPrefab ( weapons[i].sitem.Prefab, SpawnCoin, UnSpawnCoin );
        }
        
      
    }
    [Server]
    private void Start ( )
    {
        InvokeRepeating ( "SpawnTrees" , 5 ,5 );
    }
  
    public GameObject SpawnCoin ( SpawnMessage msg )
    {
        print ( "intanciao client");
        return Instantiate ( treePrefab , msg.position , msg.rotation );
    }

    public void UnSpawnCoin ( GameObject spawned )
    {
        Destroy ( spawned );
    }
    void SpawnTrees ( )
    {
        if ( MaxWeapons ( ) < 5)
        {
            GameObject treeGo = Instantiate(treePrefab,transform.position , Quaternion.identity);
            NetworkServer.Spawn ( treeGo );
        }
                   
    }

    public int MaxWeapons ( )
    {

        int cont = 0;

        foreach ( var item in NetworkServer.spawned.Values )
        {
            Item weapon = item.GetComponent<Item>();
            if ( weapon )
            {
                cont++;
            }
        }
        return cont;
    }

}
