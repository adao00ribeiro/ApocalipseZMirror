using DarkRift;
using DarkRift.Server;
using System;

public class ClientConnection 
{
    
    public IClient Client { get; }

    private Room Room;
    public ServerPlayer Player { get; set; }

    private DadosUsuario Dados;

    public ClientConnection(IClient client, LoginRequestData data)
    {
        Client = client;
        Client.MessageReceived += OnMessage;
    }
    private void OnMessage(object sender, MessageReceivedEventArgs e)
    {
        IClient client = (IClient)sender;

        using (Message message = e.GetMessage())
        {
            switch ((Tags)message.Tag)
            {
                case Tags.LobbyJoinRoomRequest:
                    RoomManager.Instance.TryJoinRoom(this, message.Deserialize<JoinRoomRequest>());
                    break;
                case Tags.LobbyGetRoom:
                    //Pega informacoes das salas
                    using (Message m = Message.Create((ushort)Tags.LobbyGetRoom, new LoginInfoData(client.ID, new LobbyInfoData(RoomManager.Instance.GetRoomDataList()))))
                    {
                        client.SendMessage(m, SendMode.Reliable);
                    }
                    break;
                case Tags.GameJoinRequest:
                  //  Room.JoinPlayerToGame(this);
                    break;
                case Tags.GamePackageClient:

                  //  Player.RecievePackage(message.Deserialize<PackageClient>());
                    break;
                case Tags.GameDisconnected:
                    //   Room.RemovePlayerFromRoom(this);
                    EmiteMessage(Tags.GameDisconnected , new LobbyInfoData(RoomManager.Instance.GetRoomDataList()),SendMode.Reliable);
                
                    break;
            }
        }
    }

    public void OnClientDisconnect(object sender, ClientDisconnectedEventArgs e)
    {
        e.Client.MessageReceived -= OnMessage;
    }

    public void EmiteMessage(Tags tag , INetworkingData data,SendMode sendmode)
    {
        using (Message m = Message.Create((ushort)tag, data))
        {
            Client.SendMessage(m, sendmode);
        }
    }

    public void EmiteMessageEmpty(Tags tag)
    {
        using (Message m = Message.CreateEmpty((ushort)tag))
        {
            Client.SendMessage(m, SendMode.Reliable);
        }
    }
    public void SetRoom(Room _room)
    {
        Room = _room;
    }

    public DadosUsuario GetDadosUsuario()
    {
        return Dados;
    }
    public void SetDadosUsuario(DadosUsuario _dados)
    {
        Dados = _dados;
    }
}
