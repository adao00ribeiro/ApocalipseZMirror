using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace ApocalipseZ
{
    public enum SpawType
    {
        ENABLE,
        DISABLE,
        RANDOM
    }
    public class SpawEnemy : NetworkBehaviour
    {
       public SpawType SpawType;
       public List<Vector3> LisPointSpaw = new List<Vector3>();

        private static SpawEnemy _instance;
        public static SpawEnemy Instance
        {
            get
            {

                return _instance;
            }
        }
        // Start is called before the first frame update
        void Start ( )
        {
            if ( _instance != null && _instance != this )
            {
                Destroy ( this.gameObject );
            }
            else
            {
                _instance = this;
            }
            if (SpawType == SpawType.ENABLE)
            {
                foreach ( Transform item in transform )
                {
                    PointSpawEnemy point = item.gameObject.GetComponent<PointSpawEnemy> ( );
                    LisPointSpaw.Add ( point.transform.position );

                    Timer.Instance.Add ( ( ) =>
                    {
                        Spawn ( point.GetPrefab ( ) , 1 );
                        Destroy ( point.gameObject );
                    } , Random.Range ( 1 , 20 ) );
                }
            }
           
        }
        public void Spawn ( GameObject prefab , int count)
        {
            for ( int i = 0 ; i < count ; i++ )
            {
                GameObject treeGo = Instantiate(prefab, GetPoint() , Quaternion.identity);
                NetworkServer.Spawn ( treeGo );
            }
            print ( "spaw de zombie");
        }

        private Vector3 GetPoint ( )
        {
            return LisPointSpaw[Random.Range(0, LisPointSpaw.Count)];
        }
    }
}