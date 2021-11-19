using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactablePickup : interactableObject
{
    protected float gameTimer;
    public GameObject objectToReveal;
    protected bool collected;
    protected Dictionary<float, InteractablePickupStruct> resetPoints;
    private ParticleSystem smokeParticle;
    protected override void Start()
    {

        eventSystem.current.updateTime += updateTimer;
        eventSystem.current.addResetPointPlayer += addResetPoints;
        eventSystem.current.returnToPoint += resetToPoint;
        initialPosition = gameObject.transform.position;
        initialRotation = transform.eulerAngles;
        resetPoints = new Dictionary<float, InteractablePickupStruct>();
        smokeParticle = GetComponentInChildren<ParticleSystem>();

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

        InteractablePickupStruct savedValues = new InteractablePickupStruct(
       collected
       );
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

    public void playerHasCollected()
    {
        itemToggler(true);
    }

    public void itemToggler(bool value)
    {
        collected = value;
        objectToReveal.SetActive(value);
        gameObject.GetComponent<BoxCollider>().enabled = !value;
        gameObject.transform.GetChild(0).gameObject.SetActive(!value);
        playSmokeParticle();

    }

    protected override void resetToInitialPoint()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = Quaternion.Euler(initialRotation);
        itemToggler(false);
    }

    protected void resetToThisTransform(InteractablePickupStruct target)
    {


        if (!target.itemCollected)
        {
            itemToggler(target.itemCollected);

        }

    }
    public void playSmokeParticle()
    {

        if (!smokeParticle.isPlaying)
        {
            smokeParticle.Play();
        }
        else
        {
            smokeParticle.Stop();
            smokeParticle.Play();
        }
    }



    public void updateTimer(float gameTimerInput)
    {
        gameTimer = gameTimerInput;
    }

    public void onDestroy()
    {
        eventSystem.current.updateTime -= updateTimer;
        eventSystem.current.addResetPointPlayer -= addResetPoints;
    }
}
