using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TypeEnemy
{
    ZOMBIE
}
[System.Serializable]
public struct Enemy
{
    public string Name;
    public GameObject Prefab;
}


[CreateAssetMenu ( fileName = "SCP_" , menuName = "ScriptableObjects/Enemys" , order = 1 )]
public class ScriptableEnemys : ScriptableObject
{
    public Enemy[] Enemys;

    public GameObject GetPrefab ( TypeEnemy type )
    {
        return Enemys[Random.Range ( 0 , Enemys.Length )].Prefab;
;    }
}
