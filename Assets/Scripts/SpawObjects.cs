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
  
  
    private void Start ( )
    {
        Transform ListSpawPointItems = GameObject.Find("ListSpawPointItems").transform;

        foreach ( Transform item in ListSpawPointItems )
        {
            PointItem point = item.gameObject.GetComponent<PointItem> ( );

            Timer.Instance.Add ( ( ) => {

                Spawn ( point.GetPrefab ( ) , point.transform.position );
                Destroy ( point.gameObject);           
            } , Random.Range(1,20));
        }
    }
    public static void Spawn( GameObject prefab , Vector3 pointSpawn)
    {
       
            GameObject treeGo = Instantiate(prefab,pointSpawn , Quaternion.identity);
            NetworkServer.Spawn ( treeGo );
               
    }

    private static int MaxWeapons ( )
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
