using System;
using System.Collections.Generic;
using DarkRift;

public struct LobbyInfoData : INetworkingData
{
    public RoomData[] Rooms;

    public LobbyInfoData(RoomData[] rooms)
    {
        Rooms = rooms;
    }

    public void Deserialize(DeserializeEvent e)
    {
        Rooms = e.Reader.ReadSerializables<RoomData>();
    }

    public void Serialize(SerializeEvent e)
    {
        e.Writer.Write(Rooms);
    }
}