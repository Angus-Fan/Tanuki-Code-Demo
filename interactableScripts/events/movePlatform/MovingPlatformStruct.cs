using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformStruct
{
    public Vector3 transformPosition;
    public Quaternion transformRotation;
    public bool isActive;

    public MovingPlatformStruct(Vector3 position, Quaternion rotation, bool activity)
    {
        transformPosition = position;
        transformRotation = rotation;
        isActive = activity;
    }
}
