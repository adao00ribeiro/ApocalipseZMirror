using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PerfilUi : MonoBehaviour
{
   
    [Header("Text")]
    public Text TextUserName;
    public Text TextAp;
    public Text TextDp;

    public Button BtnLoja;
    public Button BtnOpcao;
    public Button BtnSair;

    User user;
    private void Awake ( )
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        BtnSair.onClick.AddListener(()=>
        {
            print("Saindo");
            Application.Quit();

        });
    }

    // Update is called once per frame
    void Update()
    {
        user = GameObject.FindObjectOfType<InformacaoClient> ( ).userdata;
        TextUserName.text = user.Username;
        TextAp.text = user.Ap.ToString();
        TextDp.text = user.Dp.ToString();
    }
}
