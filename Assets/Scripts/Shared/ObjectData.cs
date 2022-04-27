using DarkRift;
using UnityEngine;

public enum Tags
{
    //Login Tags
    LoginRequest = 0,
    LoginRequestAccepted = 1,
    LoginRequestDenied = 2,
    //Lobby Tags
    LobbyJoinRoomRequest = 100,
    LobbyJoinRoomDenied = 101,
    LobbyJoinRoomAccepted = 102,
    LobbyGetRoom = 103,
    //Game Tags
    GameJoinRequest = 200,
    GameStartDataResponse = 201,
    GamePackageClient = 203,
    GamePackageServer = 204,
    GameDisconnected = 205,

    teste = 998,
    PingTag = 999,
}

public class ObjectData:  INetworkingData
{
    [Header("INFO MUNDO")]
    public string IdServe;
     public string IdPrefab;
     public Vector3 Position;
     public Quaternion Rotation;
    public virtual void Deserialize(DeserializeEvent e)
    {
        IdServe = e.Reader.ReadString();
        IdPrefab = e.Reader.ReadString();
        Position = new Vector3(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
        Rotation = new Quaternion(e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle(), e.Reader.ReadSingle());
    }

    public virtual void Serialize(SerializeEvent e)
    {
        e.Writer.Write(IdServe);
        e.Writer.Write(IdPrefab);

        e.Writer.Write(Position.x);
        e.Writer.Write(Position.y);
        e.Writer.Write(Position.z);

        e.Writer.Write(Rotation.x);
        e.Writer.Write(Rotation.y);
        e.Writer.Write(Rotation.z);
        e.Writer.Write(Rotation.w);
   
    }
}





