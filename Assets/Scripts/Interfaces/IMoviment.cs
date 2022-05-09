using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoviment
{
    void Move ( );
    void Jump ( );
    bool CheckMovement ( );
    bool isGrounded ( );

    void UpdateMoviment ( );
}
