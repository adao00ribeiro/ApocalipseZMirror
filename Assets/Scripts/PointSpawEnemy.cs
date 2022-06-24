using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ {
    public class PointSpawEnemy : MonoBehaviour
    {
        public TypeEnemy typeEnemy;

        ScriptableEnemys scpEnemy;
        public void Start ( )
        {
            scpEnemy = ScriptableManager.Instance.GetDataEnemys ( );
        }
        public GameObject GetPrefab ( )
        {
            return scpEnemy.GetPrefab( typeEnemy );
        }
        private void OnDrawGizmos ( )
        {
            Gizmos.color = Color.green;

            Gizmos.DrawCube ( transform.position + new Vector3(0,1,0)  , new Vector3(1,2,1));
        }
    }
}
