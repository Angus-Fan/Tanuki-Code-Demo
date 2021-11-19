using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct interactableRBStruct
{
    public Vector3 transformPosition;
    public Quaternion transformRotation;
    public Vector3 rbVelocity;
    public Vector3 rbAngularVelocity;

    public interactableRBStruct(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        transformPosition = position;
        transformRotation = rotation;
        rbVelocity = velocity;
        rbAngularVelocity = angularVelocity;
    }
}
