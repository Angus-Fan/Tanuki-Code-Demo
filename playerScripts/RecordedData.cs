using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordedData
{
    public Transform recordedTransform;
    public Vector3 transformPosition;
    public Vector3 transformRotation;
    public Vector3 recordedVelocity;
    public float recordedJumpSpeed;

    public RecordedData(Vector3 inputTransformPosition, Vector3 inputTransformRotation, Vector3 inputVelocity, float inputJumpSpeed)
    {
        //recordedTransform = inputTransform;
        transformPosition = inputTransformPosition;
        transformRotation = inputTransformRotation;
        recordedVelocity = inputVelocity;
        recordedJumpSpeed = inputJumpSpeed;
    }
}
