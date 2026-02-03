using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddleController : PaddleController
{
    protected override float GetInputAxis()
    {
        return Input.GetAxis("RightPaddle");
    }
}
