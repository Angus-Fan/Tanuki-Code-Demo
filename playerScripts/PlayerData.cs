using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    protected float gameTimer;
    [SerializeField]
    public Dictionary<float, PlayerDataStruct> resetPoints;
    //Player Available Skills
    [SerializeField]
    private bool canClone;
    private bool initialCanClone;

    //Player Related Values
    [SerializeField]
    private int manaValue;
    [SerializeField]
    private int maxManaValue;
    private bool hasCollectible;
    //Spawned Time values are for clones
    private int spawnedManaValue;
    private float spawnedTimeValue;

    private void Start()
    {

        initialization();
        resetPoints = new Dictionary<float, PlayerDataStruct>();
        eventSystem.current.updateTime += updateTimer;
        eventSystem.current.addResetPointPlayer += addResetPoints;
        eventSystem.current.playerManaDataUpdate += dataReset;
    }

    public void setHasChallengeCollectible(bool value)
    {
        hasCollectible = value;
    }

    public bool getChallengeCollectible()
    {
        return hasCollectible;
    }
    public void updateTimer(float gameTimerInput)
    {
        gameTimer = gameTimerInput;
    }
    protected void addResetPoints()
    {

        PlayerDataStruct savedValues = new PlayerDataStruct(
        hasCollectible,
        manaValue,
        canClone
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

    protected void dataReset(float levelTimer)
    {
        //This first check is only important for the original clone
        if (0 == levelTimer || spawnedTimeValue == levelTimer)
        {
            //Debug.Log("initialPoint");
            resetToInitialValues();
        }
        else if (resetPoints.ContainsKey(levelTimer))
        {
            //Debug.Log("NotinitialPoint");
            resetToThisPoint(resetPoints[levelTimer]);
        }
    }
    protected void resetToInitialValues()
    {
        if (gameObject.tag == "Player")
        {
            manaValue = maxManaValue;
        }
        else
        {
            manaValue = spawnedManaValue;
        }
        canClone = initialCanClone;
        hasCollectible = false;
    }
    protected void resetToThisPoint(PlayerDataStruct target)
    {


        if (!target.itemCollected)
        {
            hasCollectible = target.itemCollected;
        }

        manaValue = target.currentMana;
    }

    protected void initialization()
    {
        setManaToMax();
        initialCanClone = canClone;
        hasCollectible = false;

    }

    public void setManaToMax()
    {
        //This only happens if someone decides to change some of the inspector values
        if (manaValue > maxManaValue)
        {
            maxManaValue = manaValue;
        }
        //if it's the player and only the player initialize manaToMax
        if (gameObject.tag == "Player")
        {
            manaValue = maxManaValue;
        }
        else
        {
            spawnedManaValue = manaValue;
        }

    }

    public int getManaValue()
    {
        return manaValue;
    }
    public int getMaxManaValue()
    {
        return maxManaValue;
    }

    public void reduceMana(int i)
    {
        manaValue -= i;
    }
    public void setCanClone(bool value)
    {
        canClone = value;
    }

    public bool getCanClone()
    {
        return canClone;
    }

    //Returns true and consumes the mana, if there is enough mana to use
    //Returns false otherwise.
    public bool canUseMana(int i)
    {
        if (manaValue - i >= 0)
        {

            return true;
        }
        else
        {
            return false;
        }
    }

    public void setInitialTime(float timeValue)
    {
        spawnedTimeValue = timeValue;
    }

    public void onDestroy()
    {
        eventSystem.current.updateTime -= updateTimer;
        eventSystem.current.addResetPointPlayer -= addResetPoints;
        eventSystem.current.playerManaDataUpdate -= dataReset;
    }

}
