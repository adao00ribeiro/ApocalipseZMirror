using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
public class RoomListObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text slotsText;
    [SerializeField]
    private Button joinButton;

    public void Set( servidor data)
    {
        nameText.text = data.name;
       // slotsText.text = data.Slots + "/" + data.MaxSlots;
        joinButton.onClick.RemoveAllListeners();
        //  joinButton.onClick.AddListener(delegate { lobbyManager.SendJoinRoomRequest(data.Name); });
        joinButton.onClick.AddListener(delegate {
            // mudar para esse futuramente SceneController.Instance.CarregarCenaAsync("Mundo");
            // SceneManager.LoadSceneAsync("Mundo", LoadSceneMode.Single); 
          
            GameObject.FindObjectOfType<NetworkManager> ( ).networkAddress = data.ip;

            GameObject.FindObjectOfType<NetworkManager> ( ).StartClient ( );
        
        });

        
    }
}

