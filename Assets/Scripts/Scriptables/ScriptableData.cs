using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Informacao
{
    [SerializeField] private Sprite Avatar;
    [SerializeField] private int AP;
    [SerializeField] public GameObject Prefab;

    public Sprite GetAvatar()
    {
        return Avatar;
    }
    public void SetAp(int _ap)
    {
        AP = _ap;
    }
    public int GetAp()
    {
        return AP;
    }
}
[CreateAssetMenu(fileName = "DATA_", menuName = "ScriptableObjects/Data", order = 2)]

public class ScriptableData : ScriptableObject
{
    [SerializeField] Informacao[] Informacoes;
  
    public GameObject GetGameObject(int id)
    {
        return Informacoes[id].Prefab;
    }
    public int Count
    {
        get
        {
            return Informacoes.Length;
        }
    }
    public Sprite GetSpriteAvatar(int id)
    {
        return Informacoes[id].GetAvatar();
    }
    public void SetAp(int id , int ap)
    {
        Informacoes[id].SetAp(ap);
    }
    public int GetAp(int id)
    {
        return Informacoes[id].GetAp();
    }

}
