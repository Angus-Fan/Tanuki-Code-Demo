using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct playerInputStruct
{

    public float verticalInput;
    public float horizontalInput;
    public float cameraAngleYInput;
    public bool jumped;
    public bool running;

    public playerInputStruct(float horizontalValue, float verticalValue, bool jumpPressed, bool runPressed, float cameraAngleY)
    {
        verticalInput = verticalValue;
        horizontalInput = horizontalValue;
        cameraAngleYInput = cameraAngleY;
        jumped = jumpPressed;
        running = runPressed;
    }

    public void clear()
    {
        verticalInput = 0;
        horizontalInput = 0;
        cameraAngleYInput = 0;

    }
}
