using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TipoMeshPersonagem
{
    DEFAULT,
    CABELO,
    CAMISETA,
    LUVA,
    CALCA,
    SAPATO
}
public class CustomizacaoManager : MonoBehaviour,IPainelUpdate
{
    //68.9 fild

    public CameraCustomizacao CameraCustomizacao;
    
    public Transform personagem;
    public TipoMeshPersonagem TIPO;

    public Vector3 posicaoDefault;
    public Vector3 posicaoCabelo;
    public Vector3 posicaoCamiseta;
    public Vector3 posicaoLuva;
    public Vector3 posicaoCalca;
    public Vector3 posicaoSapato;

    private void OnEnable()
    {
        CameraCustomizacao.state = stateCamera.CUSTOMIZACAO;
    }
    private void OnDisable()
    {
        CameraCustomizacao.state = stateCamera.INICIAL;
    }
    // Update is called once per frame
    void Update()
    {
       
        switch (TIPO)
        {
            case TipoMeshPersonagem.CABELO:

                CameraCustomizacao.SetZoom(posicaoCabelo,30);
               
                break;
            case TipoMeshPersonagem.CAMISETA:
                CameraCustomizacao.SetZoom(posicaoCamiseta, 30);
                break;
            case TipoMeshPersonagem.LUVA:
                CameraCustomizacao.SetZoom(posicaoLuva, 30);
                break;
            case TipoMeshPersonagem.CALCA:
                CameraCustomizacao.SetZoom(posicaoCalca, 30);
                break;
            case TipoMeshPersonagem.SAPATO:
                CameraCustomizacao.SetZoom(posicaoSapato, 30);
                break;
            default:
                CameraCustomizacao.SetZoom(posicaoDefault, 70);
                break;
        }
      

    }

    public void SetTipoMesh(int tipo)
    {
        TIPO = (TipoMeshPersonagem)tipo;
    }

    public void Atualizar()
    {
        
    }
}
