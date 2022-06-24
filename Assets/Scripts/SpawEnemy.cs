using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public class SpawEnemy : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start ( )
        {
          

            foreach ( Transform item in transform )
            {
                PointSpawEnemy point = item.gameObject.GetComponent<PointSpawEnemy> ( );

                Timer.Instance.Add ( ( ) =>
                {

                    Spawn ( point.GetPrefab ( ) , point.transform.position );
                    Destroy ( point.gameObject );
                } , Random.Range ( 1 , 20 ) );
            }
        }
        public static void Spawn ( GameObject prefab , Vector3 pointSpawn )
        {
            GameObject treeGo = Instantiate(prefab,pointSpawn , Quaternion.identity);
            NetworkServer.Spawn ( treeGo );
        }
      
    }
}