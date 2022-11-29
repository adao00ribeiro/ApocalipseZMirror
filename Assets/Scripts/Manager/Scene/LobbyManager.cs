using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class LobbyManager : MonoBehaviour
{

    public void SelectSwat()
    {
        PlayerPrefs.SetString("NamePlayer", "Swat");

    }
    public void SelectPedro()
    {
        PlayerPrefs.SetString("NamePlayer", "Pedro");
    }
}
