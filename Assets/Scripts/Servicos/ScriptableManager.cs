using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager : MonoBehaviour
{

    public ScriptableData DataPersonagens;

    // Start is called before the first frame update
    void Awake()
    {
      
        DontDestroyOnLoad(gameObject);
    }
    public void InstanciaPersonagem(int id , Transform parent)
    {
        PersonalizaPersonagem temp = Instantiate(DataPersonagens.GetGameObject(id), parent).GetComponent<PersonalizaPersonagem>() ;
        temp.idprefab = id;
    }
}
