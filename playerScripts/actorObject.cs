using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actorObject : MonoBehaviour
{
    //What do actor objects need?
    //List-----------------------
    //1. Player Input
    //   Need a player input class to record inputs that are being sent to character.
    //2. Object Controller
    //   This object controller needs to read inputs and apply them to object.
    //3. Recording System / Playback System
    //   Recording system will need to record inputs from the player and then be able to play it back to the object


    //1 
    private playerMovementScript playerInput;

    //2
    private characterControllerScript characterControllerScript;

    //3
    private inputRecorder inputRec;
    public GameObject meshRenderer;
    public GameObject tanukiModel;
    public bool isAtInitialPoint = false;
    public Material cloneMaterial;
    private GameObject initialPos;
    private Vector3 initialPosPoint;
    public float initialTime;
    private float finalTime = 0;
    public Vector3 initialVelocity;
    private Dictionary<float, RecordedData> resetPoints;
    private float gameTimer;
    private bool isActive = true;
    private PlayerData playerData;
    public GameObject parent;
    private bool isCloneDead = false;
    private ParticleSystem smokeParticle;
    public enum state
    {
        Playing,
        Playback,
        Reset
    }
    public state currentState;

    void Start()
    {
        smokeParticle = GetComponentInChildren<ParticleSystem>();
        eventSystem.current.updateTime += updateTimer;
        playerInput = GetComponent<playerMovementScript>();
        characterControllerScript = GetComponent<characterControllerScript>();
        inputRec = GetComponent<inputRecorder>();
        resetPoints = new Dictionary<float, RecordedData>();
        // meshRenderer = GetComponent<MeshRenderer>();
        currentState = state.Playing;
        playerData = GetComponent<PlayerData>();
        playSmokeParticle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        switch (currentState)
        {
            //If you're playing you should also be recording
            case state.Playing:
                playerInput.listenForInput();
                playerInputStruct playerInputs = playerInput.getInputStruct();
                characterControllerScript.move(playerInputs);
                inputRec.addToDictionary(gameTimer, playerInputs);
                finalTime = gameTimer;
                break;
            case state.Playback:
                if (inputRec.keyExists(gameTimer))
                {
                    shouldShowClone();
                    playerInputStruct playBackInputs = inputRec.getRecordedInputs(gameTimer);
                    characterControllerScript.move(playBackInputs);
                }
                else
                {
                    if (gameTimer > finalTime)
                    {
                        shouldShowClone();
                    }
                    if (characterControllerScript.getCharacterCont().enabled == true)
                    {
                        characterControllerScript.emptyMove();
                    }
                }
                break;
            default:
                //Debug.Log("shouldn't ever be here");
                break;
        }

    }


    public void setInitialPoint(GameObject initialPoint)
    {
        initialPos = initialPoint;
    }
    public void setInitialTime(float time)
    {
        initialTime = time;
    }
   
    public void resetToInitialPoint()
    {

        GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = initialPos.transform.position;
        gameObject.transform.rotation = initialPos.transform.rotation;
        isAtInitialPoint = true;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<characterControllerScript>().resetToTheseValues(initialVelocity.y);
        hideClone();
    }
    public void returnWithThisData(RecordedData data)
    {
        GetComponent<CharacterController>().enabled = false;
        gameObject.transform.position = data.transformPosition;
        gameObject.transform.rotation = Quaternion.Euler(data.transformRotation.x, data.transformRotation.y, data.transformRotation.z);
        GetComponent<CharacterController>().enabled = true;
        GetComponent<characterControllerScript>().resetToTheseValues(data.recordedJumpSpeed);
        hideClone();
        isAtInitialPoint = false;
    }

    public void addResetPoints(RecordedData data)
    {


        //If a recording at the current time exists, then just overwrite it
        if (resetPoints.ContainsKey(gameTimer))
        {
            resetPoints[gameTimer] = data;
        }
        else
        {
            resetPoints.Add(gameTimer, data);
        }
    }
    public void hideClone()
    {
        isActive = false;
        meshRenderer.SetActive(false);
        characterControllerScript.toggleCharacterController(false);
    }
    public void showClone()
    {
        if (isAtInitialPoint)
        {
            isActive = true;
            meshRenderer.SetActive(true);
            gameObject.transform.position = initialPos.transform.position;
            gameObject.transform.rotation = initialPos.transform.rotation;
            characterControllerScript.toggleCharacterController(true);
        }
        else
        {
            isActive = true;
            meshRenderer.SetActive(true);
            characterControllerScript.toggleCharacterController(true);
        }
    }
    public void shouldShowClone()
    {
        if (initialTime != 0 && !isCloneDead)
        {
            if (initialTime <= gameTimer && !isActive)
            {
                if (meshRenderer.activeInHierarchy == false)
                {
                    playSmokeParticle();
                }
                showClone();

            }
        }
    }
    public void setCloneDead(bool value)
    {
        isCloneDead = value;
    }
    public Dictionary<float, RecordedData> getResetPoints()
    {
        return resetPoints;
    }

    public void updateTimer(float gameTimerInput)
    {
        gameTimer = gameTimerInput;
    }

    public bool isPlaying()
    {
        return currentState == state.Playing;
    }

    public void playing()
    {
        currentState = state.Playing;
    }

    public void playback()
    {
        currentState = state.Playback;
    }

    public void reset()
    {
        currentState = state.Reset;
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

    //Hard coded as 1 cost as of now, as I'm not sure if we're doing any other things in the future.
    public bool canClone()
    {
        if (playerData.canUseMana(1) && playerData.getCanClone())
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeToCloneMat()
    {
        tanukiModel.GetComponent<Renderer>().material = cloneMaterial;
    }


    public void onDestroy()
    {
        eventSystem.current.updateTime -= updateTimer;
    }

    public void enableTools(bool value)
    {
        //this means we should disable animations
        if (value == false)
        {
            characterControllerScript.setIdle();
        }
        gameObject.GetComponent<actorObject>().enabled = value;
        gameObject.GetComponent<playerMovementScript>().enabled = value;
        gameObject.GetComponent<inputRecorder>().enabled = value;

    }

}




