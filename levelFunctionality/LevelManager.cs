using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //levelVariables
    private static float levelTimer;
    public GameObject playerObject;
    //Used to check what players exist and which is active
    public Stack<GameObject> playerObjectQueue;

    public GameObject cineMachineCam;
    public Transform instantiationParentTransform;



    //nodeVariables
    public PlayerNode currentNode;
    public PlayerNode tempNode;
    //Clone Layer is the count of how many clones are currently tracked
    //Ex if i clone once, then this will increment, if that clone clones again
    //then my clone layer is 2 and so forth.
    private int cloneLayer;

    //levelUIVariables
    //Level Time Elements & Variables
    public GameObject ticker;
    public TextMeshProUGUI UILevelTimer;
    public int buildIndex;

    //The Level needs a reference to each actor that exists
    //public GameObject player;
    private List<PlayerNode> cloneHeadNodes;
    //Inputs Pressed
    private bool clonePressed;
    private bool cloneFinished;
    private bool cloneBack;
    public string levelCode;
    // Start is called before the first frame update


    //Change
    //Delete After
    public int counter = 0;

    void Start()
    {

        if (buildIndex != 0 || buildIndex != 1)
        {
            try
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
                LightProbes.Tetrahedralize();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            Debug.Log("Please Enter A build index To Set This as Active Scene");
        }
        PlayerPrefs.SetInt(levelCode, 1);
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Locked;
        cloneLayer = 0;
        levelTimer = 0;

        playerObjectQueue = new Stack<GameObject>();

        playerObjectQueue.Push(playerObject);
        cloneHeadNodes = new List<PlayerNode>();
        InitialNode initialNode = new InitialNode(levelTimer, createIndicator());
        currentNode = initialNode;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("backspace")) { cloneBack = true; }
        if (Input.GetKeyDown("right shift")) { clonePressed = true; }
        if (Input.GetKeyDown("right ctrl")) { cloneFinished = true; }
    }
    void FixedUpdate()
    {
        inputHandler();
        updateTime();
        updatePlayerTimers();
        clearInputs();
    }


    private void inputHandler()
    {
        // if (Input.GetKeyDown("return"))
        // {
        //     if (playerObject.GetComponent<CharacterController>().isGrounded)
        //     {
        //         createAnchorPoint();
        //     }
        // }
        if (cloneBack)
        {
            back();
        }
        if (clonePressed)
        {
            if (playerObject.GetComponent<CharacterController>().isGrounded && playerObject.GetComponent<actorObject>().canClone())
            {


                createSplitNode();
                createCloneHead();
                incrementCloneLayer();
                addCloneResetPoints();
                addObjectResetPoints();
                playerObject.GetComponent<PlayerData>().reduceMana(1);
                eventSystem.current.updateManaFunction();
                //camShakeEloel
                //CameraShake.Instance.cameraShake(1, .25f);
                spawnClone();


            }
        }
        if (cloneFinished)
        {

            if (playerObject.GetComponent<CharacterController>().isGrounded && cloneLayer != 0)
            {
                //Once a route has been completed, we need to keep track of them
                //To add midPoint resets
                createAnchorPoint();
                playerObject.GetComponent<inputRecorder>().cullExcessDictionaryKeys(currentNode.time);

                //If there is a clone of a clone the startPoint needs to be traced back
                if (currentNode is CloneHeadNode)
                {
                    cloneHeadNodes.Add(currentNode);
                }
                else
                {
                    while (!(currentNode is CloneHeadNode))
                    {
                        currentNode = currentNode.from;
                    }
                    cloneHeadNodes.Add(currentNode);
                }
                returnToSplit();
                cloneSwapToPlayer();
                cloneReturn();
                itemReturn();
                updateManaWhenCloneComplete();


            }
        }
    }
    private void clearInputs()
    {
        cloneBack = false;
        clonePressed = false;
        cloneFinished = false;
    }

    private GameObject createIndicator()
    {
        GameObject newAnchor = Instantiate(ticker, playerObject.transform.position, playerObject.transform.rotation, instantiationParentTransform);

        if (currentNode != null && !(currentNode is InitialNode))
        {
            newAnchor.GetComponentInChildren<TimerFill>().setValues(currentNode.from.time, currentNode.time);
        }
        /*
        else
        {
            newAnchor.GetComponentInChildren<TimerFill>().setValues(0, levelTimer);
        }*/


        //newAnchor.GetComponentInChildren<TextMeshProUGUI>().SetText("Time : " + levelTimer.ToString("F2") + "\n" + "From : " + currentNode);

        TimeSpan time = TimeSpan.FromSeconds(levelTimer);
        string str = time.ToString(@"mm\:ss");
        if (levelTimer > 3600)
        {
            str = time.ToString(@"hh\:mm\:ss");
        }
        newAnchor.GetComponentInChildren<TextMeshProUGUI>().SetText(str);

        return newAnchor;

    }

    private void updateCharacterPosition()
    {
        playerObject.GetComponent<CharacterController>().enabled = false;
        playerObject.transform.position = currentNode.indicator.transform.position;
        playerObject.transform.rotation = currentNode.indicator.transform.rotation;
        playerObject.GetComponent<CharacterController>().enabled = true;
        playerObject.GetComponent<characterControllerScript>().resetVals();
    }

    private void createSplitNode()
    {
        //Debug.Log("Creating Split Node");
        SplitNode split = new SplitNode(currentNode, levelTimer);
        currentNode.setToNode(split);
        currentNode = split;
        split.setIndicator(createIndicator());
    }
    private void createCloneHead()
    {
        //Debug.Log("Creating Clone Head Node");
        CloneHeadNode cloneHead = new CloneHeadNode(currentNode, levelTimer);
        currentNode.setSplitNode(cloneHead);
        currentNode = cloneHead;
        cloneHead.setIndicator(createIndicator());

    }

    private void returnToSplit()
    {

        while (!(currentNode is CloneHeadNode))
        {
            currentNode = currentNode.from;
        }
        //Return from the clone head and go back to split node
        currentNode = currentNode.from;
        returnToCurrentNode();
    }

    private void returnToCurrentNode()
    {
        levelTimeToCurrentNodeTime();
        updateCharacterPosition();

    }

    private void createAnchorPoint()
    {
        AnchorNode anchor = new AnchorNode(currentNode, levelTimer);
        currentNode.setToNode(anchor);
        currentNode = anchor;
        anchor.setIndicator(createIndicator());
    }

    private void incrementCloneLayer()
    {
        cloneLayer++;
    }
    private void decrementCloneLayer()
    {
        cloneLayer--;
    }

    private void deleteNode()
    {


        if (currentNode.from != null)
        {

            //If we're deleting a clone head, then we haven't
            //done anything as a clone, just go back to last node
            if (currentNode is CloneHeadNode)
            {
                currentNode = currentNode.from;
                cloneSwapToPlayer();
            }
            //If it's a split node then we gotta check and
            //delete the other split timetrcks
            if (currentNode is SplitNode)
            {
                clearToRow();
                clearChildren(currentNode.split);
            }
            currentNode.indicator.GetComponentInChildren<TimerFill>().onDestroy();
            Destroy(currentNode.indicator);
            tempNode = currentNode.from;
            tempNode.to = null;
            currentNode = tempNode;
        }
    }

    public void back()
    {
        //If we're going back we pretty much need to refund mana
        if (levelTimer - currentNode.time >= .5)
        {
            //Debug.Log("ME RETURN");


            //9/28/2021 
            //Added a cull excess to the clones on complete
            //If backspace is hit, there's no need to actually clear the dictionary
            //If there is some logical error you can uncomment the line below
            //If the line below is uncommented, then if backspace is used on the main
            //Clone then the final replay will delete any previous actions.. please beware.
            // playerObject.GetComponent<inputRecorder>().resetDictionary();
            returnToCurrentNode();
            itemReturn();
            cloneReturn();
            eventSystem.current.updatePlayerManaData(levelTimer);
            eventSystem.current.updateManaFunction();

        }
        else
        {
            //Debug.Log("ME DELETE");
            deleteNode();
            returnToCurrentNode();

            itemReturn();
            cloneReturn();
            eventSystem.current.updatePlayerManaData(levelTimer);
            eventSystem.current.updateManaFunction();

        }
    }


    private void clearToRow()
    {
        PlayerNode iterationNode = currentNode.to;
        while (iterationNode != null)
        {
            PlayerNode nextNode = iterationNode.to;
            if (iterationNode is SplitNode)
            {
                clearChildren(iterationNode);
            }

            iterationNode.to = null;
            Destroy(iterationNode.indicator);
            //If theres a clone and the track is being deleted delete the clone
            if (iterationNode.getCloneField() != null)
            {
                iterationNode.getCloneField().GetComponent<actorObject>().onDestroy();
                cloneHeadNodes.Remove(iterationNode);
                iterationNode.getCloneField().GetComponent<PlayerData>().onDestroy();
                Destroy(iterationNode.getCloneField());
            }
            iterationNode = nextNode;

        }

    }

    private void clearChildren(PlayerNode startNode)
    {
        // o - o - o
        //     | 
        //     o - o - o
        //         |
        //         o - o

        // o - o - o
        //     |
        //     / - / - o
        //         |
        //         / - / 


        ArrayList splitNodesPast = new ArrayList();
        PlayerNode iterationNode = startNode;

        /*
                while (iterationNode != null)
                {
                    if (iterationNode is SplitNode)
                    {
                        splitNodesPast.Add(iterationNode);
                    }
                    iterationNode = iterationNode.to;

                }
                Debug.Log(splitNodesPast.Count);*/



        while (iterationNode != null)
        {
            if (iterationNode is SplitNode)
            {

                splitNodesPast.Add(iterationNode);
                iterationNode = iterationNode.split;

            }
            else
            {
                PlayerNode nextNode = iterationNode.to;
                iterationNode.to = null;
                Destroy(iterationNode.indicator);
                //If theres a clone and the track is being deleted delete the clone
                if (iterationNode.getCloneField() != null)
                {
                    iterationNode.getCloneField().GetComponent<actorObject>().onDestroy();
                    cloneHeadNodes.Remove(iterationNode);
                    iterationNode.getCloneField().GetComponent<PlayerData>().onDestroy();
                    //need to disable the hitbox before deleting the character
                    iterationNode.getCloneField().GetComponent<CharacterController>().enabled = false;
                    Destroy(iterationNode.getCloneField());
                }
                iterationNode = nextNode;
            }
        }
        foreach (SplitNode n in splitNodesPast)
        {
            clearChildren(n.to);
            Destroy(n.to.indicator);
            Destroy(n.indicator);
        }
    }

    private void levelTimeToCurrentNodeTime()
    {
        levelTimer = currentNode.time;
    }
    private void cloneSwapToPlayer()
    {
        decrementCloneLayer();
        playerReturn();
    }


    private void playerReturn()
    {
        //When returning back to the previous player,
        //Set the current FINISHED clone to playback
        //Pop the queue to get the previous clone's creator

        playerObject.GetComponent<actorObject>().playback();
        playerObjectQueue.Pop();
        playerObject = playerObjectQueue.Peek();

        //Set the parent back to playing state
        //playerObject.SetActive(true);
        //playerObject.GetComponent<actorObject>().playing();
        enableCurrentPlayer(true);

        playerObject.GetComponent<actorObject>().playSmokeParticle();

        //cinemachine camera updates
        cinemachineUpdate(playerObject.transform);
    }
    private void spawnClone()
    {
        //In this following section, when cloning, set the clone to 'reset' state.
        //This state ensures that no code is ran, however their hitbox, visuals are still there
        //This should only be called after the clone node has been created (currentNode === clone node)
        //playerObject.SetActive(false);
        enableCurrentPlayer(false);


        //Afterwards we're creating a new clone based on the old clones data
        GameObject newClone = Instantiate(playerObject, currentNode.indicator.transform.position, currentNode.indicator.transform.rotation, instantiationParentTransform);

        //Debug Stuff, to make sure the clones are correc in the insepctor
        newClone.name = (++counter).ToString();
        newClone.tag = "Clone";

        //Setting up initial values on the new clone
        actorObject newCloneAO = newClone.GetComponent<actorObject>();
        newCloneAO.setInitialPoint(playerObject);
        newCloneAO.setInitialTime(currentNode.time);
        newCloneAO.changeToCloneMat();
        newClone.GetComponent<PlayerData>().setInitialTime(currentNode.time);

        //Updating object queue to ensure that we know which player is acting
        playerObjectQueue.Push(newClone);
        playerObject = playerObjectQueue.Peek();
        currentNode.setCloneField(playerObject);

        //Set the value of the new cloen to playing, so it can use code
        //playerObject.SetActive(true);
        enableCurrentPlayer(true);

        //Cinemachine camera updates
        cinemachineUpdate(newClone.transform);
    }

    private void enableCurrentPlayer(bool val)
    {
        playerObject.GetComponent<actorObject>().enableTools(val);
    }

    private void cinemachineUpdate(Transform targetTransform)
    {
        cineMachineCam.GetComponent<CinemachineFreeLook>().Follow = targetTransform;
        cineMachineCam.GetComponent<CinemachineFreeLook>().LookAt = targetTransform;
    }

    private void cloneReturn()
    {
        //Debug.Log("currently there are [" + cloneHeadNodes.Count + "] playerNodes");
        foreach (PlayerNode p in cloneHeadNodes)
        {
            GameObject targetClone = p.getCloneField();
            //Debug.Log(targetClone);
            Dictionary<float, RecordedData> resetPoints = p.getCloneField().GetComponent<actorObject>().getResetPoints();
            //Check to see if the initial point of the node is the node we returned to
            //If it is then go ahead and resetToInitialPoint
            if (p.time == levelTimer || p.time > levelTimer)
            {


                targetClone.GetComponent<actorObject>().resetToInitialPoint();

            }
            else if (resetPoints.ContainsKey(levelTimer))
            {


                targetClone.GetComponent<actorObject>().returnWithThisData(resetPoints[levelTimer]);


            }
        }
    }
    //This is from an era where you could put 'save points' to backspace too, this was cut early
    //This probably won't work if you just uncomment a bit of code, but I'm leaving it here in case
    //This ever becomes something that needs to be used
    /*
        private void cloneReturn()
    {
        //Debug.Log("currently there are [" + cloneHeadNodes.Count + "] playerNodes");
        foreach (PlayerNode p in cloneHeadNodes)
        {
            GameObject targetClone = p.getCloneField();
            //Debug.Log(targetClone);
            Dictionary<float, Transform> resetPoints = p.getCloneField().GetComponent<actorObject>().getResetPoints();
            //Check to see if the initial point of the node is the node we returned to
            //If it is then go ahead and resetToInitialPoint
            if (p.time == levelTimer || p.time > levelTimer)
            {
                targetClone.GetComponent<actorObject>().resetToInitialPoint();
            }
            else if (resetPoints.ContainsKey(levelTimer))
            {
                targetClone.GetComponent<actorObject>().resetToThisTransform(resetPoints[levelTimer]);
            }
        }
    }*/

    private void updateManaWhenCloneComplete()
    {
        PlayerDataStruct tempValues = playerObject.GetComponent<PlayerData>().resetPoints[levelTimer];
        playerObject.GetComponent<PlayerData>().resetPoints[levelTimer] = new PlayerDataStruct(tempValues.itemCollected, tempValues.currentMana - 1, tempValues.canClone);
        eventSystem.current.updatePlayerManaData(levelTimer);
        eventSystem.current.updateManaFunction();
    }
    private void itemReturn()
    {
        eventSystem.current.resetToPoint(levelTimer);
    }
    private void updateTime()
    {
        voidUpdateLevelTimer();
        updateLevelTimerUI();
    }

    private void updatePlayerTimers()
    {
        eventSystem.current.updateTimeInvoked(levelTimer);
        //actorObject.updateTimer(levelTimer);
        //playerObject.GetComponent<actorObject>().updateTimer(levelTimer);
    }

    private void addCloneResetPoints()
    {
        //We need to make sure that when another clone is made and backspace is hit
        //there is a point where the other clones return to. and not their original positions
        //this adds 'midpoints' throughout their path to return to
        //Debug.Log("There are going to be this many midPoitns created : " + cloneHeadNodes.Count);
        foreach (PlayerNode node in cloneHeadNodes)
        {
            if (node.time < levelTimer)
            {
                GameObject clone = node.getCloneField();

                //Transform targetTransform = ;
                Vector3 targetPosition = clone.transform.position;
                Vector3 targetRotation = clone.transform.eulerAngles;
                Vector3 targetVelocity = clone.GetComponent<CharacterController>().velocity;
                float targetJumpSpeed = clone.GetComponent<characterControllerScript>().getTempJumpSpeed();
                //Debug.Log("The Transform of the clone at moment is : " + targetTransform.position);
                RecordedData targetData = new RecordedData(targetPosition, targetRotation, targetVelocity, targetJumpSpeed);
                clone.GetComponent<actorObject>().addResetPoints(targetData);
            }
        }
    }

    private void addObjectResetPoints()
    {
        eventSystem.current.recordResetPoint();
    }
    //Func: voidUpdateTimer
    //Desc: updates the levelTimer in seconds
    private void voidUpdateLevelTimer()
    {
        //Increment the level time
        levelTimer += Time.deltaTime;

    }

    private void updateLevelTimerUI()
    {


        //string twoDecimalTime = levelTimer.ToString();//.ToString("F2");
        TimeSpan time = TimeSpan.FromSeconds(levelTimer);
        string str = time.ToString(@"mm\:ss");
        if (levelTimer > 3600)
        {
            str = time.ToString(@"hh\:mm\:ss");
        }
        UILevelTimer.SetText(str);
    }


    //Func: getTime
    //out : level's timer : levelTimer
    //desc: returns the level's time
    public float getTime()
    {
        return levelTimer;
    }

    public void finishLevel()
    {

        playerObject.GetComponent<actorObject>().playback();
        while (!(currentNode is InitialNode))
        {
            currentNode = currentNode.from;
        }
        returnToCurrentNode();
        cloneReturn();
        itemReturn();
        StartCoroutine("waitForVelocityToDrop");


    }

    public void swapMeshesToOpaque()
    {
        eventSystem.current.swapMaterialToOpaque();

    }



    IEnumerator waitForVelocityToDrop()
    {
        while (!(playerObject.GetComponent<CharacterController>().velocity == Vector3.zero))
        {
            yield return null;
        }
        //Debug.Log("were in finish level");

        returnToCurrentNode();
        cloneReturn();
        itemReturn();

    }
}
