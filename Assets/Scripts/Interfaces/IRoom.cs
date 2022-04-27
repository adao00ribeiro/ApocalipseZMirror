using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom 
{
    void Initialize(string name , byte maxSlots);
    void AddPlayerToRoom(ClientConnection conn);
    void RemovePlayerToRoom(ClientConnection coon);

    void JoinPlayerToGame(ClientConnection coon);

    void Close();

    string GetName();

    byte GetMaxSlots();
    int GetMaxConnections();


}
