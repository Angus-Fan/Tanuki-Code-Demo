using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableObject : MonoBehaviour
{
    protected Vector3 initialPosition;
    protected Vector3 initialRotation;

    protected virtual void Start()
    {

    }

    public interactableObject()
    {

    }


    protected virtual void resetToInitialPoint()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = Quaternion.Euler(initialRotation);
    }

    protected virtual void resetToThisTransform(Transform target)
    {
        gameObject.transform.position = target.position;
        gameObject.transform.rotation = target.rotation;
    }

}
