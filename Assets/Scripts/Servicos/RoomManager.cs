using System.Collections.Generic;
using DarkRift;
using DarkRift.Server;
using UnityEngine;
public class RoomManager : MonoBehaviour
{

	Dictionary<string, IRoom> rooms = new Dictionary<string, IRoom>();

	public static RoomManager Instance;

	[Header("Prefabs de Rooms")]
	[SerializeField]
	private GameObject roomPrefab;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(this);
		//cria o mundo principal
		CreateRoom("Main", 25);
		//	CreateRoom("Main 2", 15);
	}

	public RoomData[] GetRoomDataList()
	{
		RoomData[] data = new RoomData[rooms.Count];
		int i = 0;
		foreach (KeyValuePair<string, IRoom> kvp in rooms)
		{
			IRoom r = kvp.Value;
			data[i] = new RoomData(r.GetName(), (byte)r.GetMaxConnections(), r.GetMaxSlots());
			i++;
		}
		return data;
	}

	public void TryJoinRoom(ClientConnection client, JoinRoomRequest data)
	{
		/*
		bool canJoin = ServerController.Instance.Players.TryGetValue(client.ID, out var clientConnection);

		if (!rooms.TryGetValue(data.RoomName, out var room))
		{
			canJoin = false;
		}
		else if (room.GetMaxConnections() >= room.MaxSlots)
		{
			canJoin = false;
		}

		if (canJoin)
		{
			room.AddPlayerToRoom(clientConnection);
		}
		else
		{
			client.EmiteMessage(Tags.LobbyJoinRoomDenied, new LobbyInfoData(GetRoomDataList()),SendMode.Reliable);
		}
		*/
	}

	public void CreateRoom(string roomName, byte maxSlots)
	{
		GameObject go = Instantiate(roomPrefab);
		Room room = go.GetComponent<Room>();
		room.Initialize(roomName, maxSlots);
		rooms.Add(roomName, room);
	}

	public void RemoveRoom(string roomName)
	{
		IRoom r = rooms[roomName];
		r.Close();
		rooms.Remove(roomName);
	}

}