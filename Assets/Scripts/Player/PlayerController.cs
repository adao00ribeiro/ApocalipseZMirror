using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;

namespace ApocalipseZ
{


    public class PlayerController : NetworkBehaviour
    {

        [SyncVar]
        private NetworkObject player;
        // Start is called before the first frame update
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (base.IsOwner)
            {
                CmdSpawCharacter(PlayerPrefs.GetString("NamePlayer"));

            }

        }

        [ServerRpc]
        public void CmdSpawCharacter(string name, NetworkConnection sender = null)
        {

            DataCharacter data = GameController.Instance.DataManager.GetCharacterByName(name);
            if (data)
            {
                player = Instantiate(data.Prefab);
                base.Spawn(player, sender);
            }

        }

        public NetworkObject GetPlayer()
        {
            return player;
        }
    }
}