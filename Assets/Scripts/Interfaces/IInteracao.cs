using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteracao 
{
    void Interacao(ServerPlayer caller);

    void StartFocus();

    void EndFocus();
}
