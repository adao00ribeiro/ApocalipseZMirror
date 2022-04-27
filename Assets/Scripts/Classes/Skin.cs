using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InformacaoSkin
{
  
    [SerializeField] private Sprite pImageSkin;
    [SerializeField] private Material pMaterialSkin;
    private bool Comprado;

    public Sprite ImageSkin
    {
        get
        {
            return pImageSkin;
        }
        set
        {
            pImageSkin = value;
        }
    }
    public Material MaterialSkin
    {
        get
        {
            return pMaterialSkin;
        }
        set
        {
            pMaterialSkin = value;
        }
    }
}
public class Skin : MonoBehaviour
{
    public SkinnedMeshRenderer render;
    public InformacaoSkin[] Skins;

    private void Start()
    {
        render = GetComponent<SkinnedMeshRenderer>();
    }
    public bool teste1, teste2;
    private void Update()
    {
        if (teste1)
        {
            SetMaterial(0);
            teste1 = false;
        }
        if (teste2)
        {
            SetMaterial(1);
            teste2 = false;
        }
    }
    public void SetMaterial(int index)
    {
        render.sharedMaterial = Skins[index].MaterialSkin;
    }

}
