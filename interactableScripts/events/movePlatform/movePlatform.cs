using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movePlatform : MonoBehaviour
{


    //Platform Related Info
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private float gameTimer;
    private Vector3 initialPlatformPos;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float travelTime;
    public LeanTweenType itemEaseType;
    //Is someone on the pressure plate?
    private bool isActive;
    private Rigidbody rb;
    private Vector3 target;
    //When resetting we need to make sure all the relevant information is included
    protected Dictionary<float, MovingPlatformStruct> resetPoints;

    [SerializeField]
    private bool carriesPlayers;
    private List<GameObject> playersRiding;
    private IEnumerator movementCoroutine;
    private AudioSource moveSound;


    public GearSpin gearSpin;
    private void Start()
    {
        eventSystem.current.returnToPoint += resetToPoint;
        eventSystem.current.updateTime += updateTimer;
        eventSystem.current.addResetPointPlayer += addResetPoints;
        eventSystem.current.levelFinishFunction += levelCompleteMute;
        rb = GetComponent<Rigidbody>();
        playersRiding = new List<GameObject>();
        initialPlatformPos = gameObject.transform.localPosition;
        initialPosition = gameObject.transform.localPosition;
        initialRotation = transform.eulerAngles;
        isActive = false;
        resetPoints = new Dictionary<float, MovingPlatformStruct>();
        try
        {
            moveSound = GetComponent<AudioSource>();
        }
        catch (Exception e)
        {
            print(e);
        }

    }

    public void moveToDestination()
    {
        // isActive = true;
        // LeanTween.cancel(gameObject);
        // LeanTween.moveLocal(gameObject, destination, travelTime);
        if (isActive != true)
        {
            isActive = true;
            float remainingTravelTime = timeToTravel(travelTime, initialPosition, destination, gameObject.transform.localPosition);
            LeanTween.cancel(gameObject);
            LeanTween.moveLocal(gameObject, destination, remainingTravelTime).setEase(itemEaseType);
            Play(moveSound, remainingTravelTime);
            spinGearsForward(remainingTravelTime);

        }
    }




    private void moveCharactersWithPlatform()
    {
        foreach (GameObject player in playersRiding)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (player.activeInHierarchy == true)
            {
                //cc.Move((rb.velocity + cc.velocity) * Time.deltaTime);
            }
        }
    }
    public void returnToStart()
    {
        //The only difference between this call and the moveToDestinationCall is the order of parameters
        //Our destination is our start point and our initialPosition is our target

        isActive = false;
        float remainingTravelTime = timeToTravel(travelTime, destination, initialPosition, gameObject.transform.localPosition);
        LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, initialPlatformPos, remainingTravelTime).setEase(itemEaseType);
        spinGearsBack(remainingTravelTime);
        stopMoveSound(moveSound);

    }
    public void backPressed()
    {
        returnToStart();
        isActive = false;
        foreach (GameObject player in playersRiding)
        {
            player.transform.parent = null;
        }
        playersRiding = new List<GameObject>();
    }

    public float timeToTravel(float travelTime, Vector3 origin, Vector3 destination, Vector3 distanceTravelled)
    {

        return travelTime - (travelTime / Vector3.Distance(destination, origin)) * (Vector3.Distance(distanceTravelled, origin));
    }
    public void checkIfOnStay(Collider testCollider)
    {
        if (isActive == false)
        {
            moveToDestination();
        }
        isActive = true;
    }
    public void checkIfOnPlatform(Collider testCollider)
    {
        if (testCollider.tag == "Player" || testCollider.tag == "Clone")
        {
            if (!playersRiding.Contains(testCollider.gameObject))
            {
                playersRiding.Add(testCollider.gameObject);
                testCollider.gameObject.transform.parent = gameObject.transform;
            }
        }
    }
    public void leftPlatform(Collider testCollider)
    {

        if (playersRiding.Contains(testCollider.gameObject))
        {
            playersRiding.Remove(testCollider.gameObject);

        }
        testCollider.gameObject.transform.parent = null;
    }
    protected void resetToPoint(float levelTimer)
    {
        if (0 == levelTimer)
        {

            resetToInitialPoint();
            backPressed();
        }
        else if (resetPoints.ContainsKey(levelTimer))
        {

            resetToThisTransform(resetPoints[levelTimer]);
            backPressed();
        }
    }

    protected void addResetPoints()
    {

        MovingPlatformStruct savedValues = new MovingPlatformStruct(
        gameObject.transform.position,
        gameObject.transform.rotation,
        isActive);
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

    protected void resetToInitialPoint()
    {
        gameObject.transform.position = initialPosition;
        gameObject.transform.rotation = Quaternion.Euler(initialRotation);
        isActive = false;
    }

    protected void resetToThisTransform(MovingPlatformStruct target)
    {
        gameObject.transform.position = target.transformPosition;
        gameObject.transform.rotation = target.transformRotation;
        isActive = target.isActive;
    }

    public void updateTimer(float gameTimerInput)
    {
        gameTimer = gameTimerInput;
    }

    public void onDestroy()
    {
        eventSystem.current.returnToPoint -= resetToPoint;
        eventSystem.current.updateTime -= updateTimer;
        eventSystem.current.addResetPointPlayer -= addResetPoints;
        eventSystem.current.levelFinishFunction -= levelCompleteMute;
    }

    public void Play(AudioSource s, float time)
    {
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("There is no sound attached to this object");
            return;
        }
        if (time < 0)
        {
            time = 0;
        }
        else if (time > s.clip.length)
        {
            time = s.clip.length;
        }

        s.time = s.clip.length - time;
        if (s.time < s.clip.length)
        {
            s.Play();
        }
    }

    public void stopMoveSound(AudioSource s)
    {
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("There is no sound attached to this object");
            return;
        }
        if (s.time < s.clip.length)
        {
            s.Stop();
        }
    }

    public void levelCompleteMute()
    {
        if (moveSound != null)
        {
            moveSound.mute = true;
        }
    }

    public void spinGearsForward(float time)
    {
        if (gearSpin != null)
        {
            gearSpin.startSpin(time);
        }
    }
    public void spinGearsBack(float time)
    {
        if (gearSpin != null)
        {
            gearSpin.returnSpin(time);
        }
    }



}
