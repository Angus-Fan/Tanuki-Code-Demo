using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableRigidBody : interactableObject
{
    protected Rigidbody rb;
    protected Vector3 initialVelocity;
    protected Vector3 initialAngularVelocity;
    protected float gameTimer;
    protected Dictionary<float, interactableRBStruct> resetPoints;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        eventSystem.current.updateTime += updateTimer;
        eventSystem.current.addResetPointPlayer += addResetPoints;
        eventSystem.current.returnToPoint += resetToPoint;
        initialPosition = gameObject.transform.position;
        initialRotation = transform.eulerAngles;
        initialVelocity = rb.velocity;
        resetPoints = new Dictionary<float, interactableRBStruct>();
    }

    protected void resetToPoint(float levelTimer)
    {
        if (0 == levelTimer)
        {
            // Debug.Log("initialPoint");
            resetToInitialPoint();
        }
        else if (resetPoints.ContainsKey(levelTimer))
        {
            //Debug.Log("NotinitialPoint");
            resetToThisTransform(resetPoints[levelTimer]);
        }
    }

    protected void addResetPoints()
    {

        interactableRBStruct savedValues = new interactableRBStruct(
        gameObject.transform.position,
        gameObject.transform.rotation,
        rb.velocity,
        rb.angularVelocity);
        //If a recording at the current time exists, then just overwrite it
        if (resetPoints.ContainsKey(gameTimer))
        {
            resetPoints[gameTimer] = savedValues;
        }
        else
        {
            resetPoints.Add(gameTimer, savedValues);
        }
    }


    protected override void resetToInitialPoint()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = Quaternion.Euler(initialRotation);
        rb.velocity = initialVelocity;
        rb.angularVelocity = initialAngularVelocity;
    }

    protected void resetToThisTransform(interactableRBStruct target)
    {

        gameObject.transform.position = target.transformPosition;
        gameObject.transform.rotation = target.transformRotation;
        rb.velocity = target.rbVelocity;
        rb.angularVelocity = target.rbAngularVelocity;
    }


    public void updateTimer(float gameTimerInput)
    {
        gameTimer = gameTimerInput;
    }

    public void onDestroy()
    {
        eventSystem.current.updateTime -= updateTimer;
        eventSystem.current.addResetPointPlayer -= addResetPoints;
        eventSystem.current.returnToPoint -= resetToPoint;

    }
}

