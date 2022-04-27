using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServidoresUI : MonoBehaviour , IPainelUpdate
{

    [Header("References")]
    [SerializeField]
    private Transform roomListContainerTransform;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject roomListPrefab;
    
    [SerializeField] private RequestApi api;
    // Start is called before the first frame update
    private void Awake()
    {
        api = GameObject.FindObjectOfType<RequestApi> ();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void Atualizar()
    {
        List<string> param = new List<string>() ;
        param.Add ("servidor");

        StartCoroutine ( api.Request<ListServer> ( param.ToArray ( ) , listparam => {

            RefreshRooms ( listparam );

        } ) );

       
    }

    public void OnRoomJoinDenied()
    {
     //   RefreshRooms(lobby.data);
    }

    public void RefreshRooms(ListServer data)
    {
        RoomListObject[] roomObjects = roomListContainerTransform.GetComponentsInChildren<RoomListObject>();

        if (roomObjects.Length > data.servers.Length)
        {
            for (int i = data.servers.Length; i < roomObjects.Length; i++)
            {
                Destroy(roomObjects[i].gameObject);
            }
        }

        for (int i = 0; i < data.servers.Length; i++)
        {
            servidor server = data.servers[i];
            if (i < roomObjects.Length)
            {
               // roomObjects[i].Set(lobby, server );
            }
            else
            {
                GameObject go = Instantiate(roomListPrefab, roomListContainerTransform);
                go.GetComponent<RectTransform> ( ).sizeDelta = new Vector2( 0 , 150 );
                go.GetComponent<RoomListObject>().Set( server );
            }
        }
    }
}
