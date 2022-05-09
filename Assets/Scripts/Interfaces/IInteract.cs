using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    void OnInteract ( IFpsPlayer player );

    void StartFocus ( );

    void EndFocus ( );
    string GetTitle ( );
}
