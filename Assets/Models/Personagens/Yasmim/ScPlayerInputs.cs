using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScPlayerInputs : MonoBehaviour
{
    public bool  runInput, walkInput, slow_WalkingInput, crouchInput, crawlInput;
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveZ != 0) walkInput = true;
        else walkInput = false;

        if (walkInput) runInput = Input.GetKey(KeyCode.LeftShift);
        else runInput = false;

        if (walkInput) slow_WalkingInput = Input.GetKey(KeyCode.CapsLock);
        else slow_WalkingInput = false;

        crouchInput = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.Z)) { crawlInput = !crawlInput; }
    }
}
