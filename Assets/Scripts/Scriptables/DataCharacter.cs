using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Character", order = 1)]
public class DataCharacter : ScriptableObject
{
    public string Name;
    public NetworkObject Prefab;
}
