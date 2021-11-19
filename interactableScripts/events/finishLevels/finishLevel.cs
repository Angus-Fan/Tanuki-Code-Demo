using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class finishLevel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Canvas levelCompleteCanvas;
    public PauseMenuScript pauseMenuScript;
    public CanvasGroup manaCanvasGroup;
    public LevelManager levelManager;
    public string levelCode = "";
    public string nextLevelCode = "";

    public int aRankTime = 5;
    public int bRankTime = 15;
    private int levelCompleteCounter = 0;
    public void levelComplete(Collider collidedObject)
    {

        if (collidedObject.tag == "Player")
        {
            //Only set all the variables on the first pass
            if (levelCompleteCounter == 0)
            {
                GameObject player = collidedObject.gameObject;
                pauseMenuScript.setGameFinished(true);
                stopPlayerInputs(player);
                levelCompleteCanvas.enabled = true;
                GetComponent<endScreen>().collectibleObtained(playerCollectedItem(player));
                //Hide Mana Icon
                manaCanvasGroup.alpha = 0;
                if (string.IsNullOrEmpty(levelCode) == false)
                {
                    //On Level Complete Save the Level Code with the '1' for level Completion;

                    eventSystem.current.levelFinish();

                    //When Level Is Complete Set the time to complete

                    string saveLevelTime = GetComponent<endScreen>().timeToString(levelManager.getTime());
                    float previousHighScore = PlayerPrefs.GetFloat(levelCode + "Time", 0);
                    if (levelManager.getTime() < previousHighScore || previousHighScore == 0)
                    {
                        //It's a new high score please save it!
                        Debug.Log("Saving " + levelCode + "Time" + " with value " + saveLevelTime);
                        PlayerPrefs.SetFloat(levelCode + "Time", levelManager.getTime());
                        GetComponent<endScreen>().showNewRecord();

                    }
                    GetComponent<endScreen>().setEndValues(levelManager.getTime(), PlayerPrefs.GetFloat(levelCode + "Time", 0));

                    //If you collect the object then set the value to 1;
                    int collectibleValue = 0;
                    if (playerCollectedItem(player) || PlayerPrefs.GetInt(levelCode + "Collectible", 0) != 0)
                    {
                        collectibleValue = 1;
                    }
                    PlayerPrefs.SetInt(levelCode + "Collectible", collectibleValue);
                    Debug.Log("Saving " + levelCode + "Collectible" + " with value " + collectibleValue);


                    float completedTime = levelManager.getTime();
                    string rank = "";
                    if (completedTime <= aRankTime)
                    {
                        rank += "A";
                    }
                    else if (completedTime <= bRankTime)
                    {
                        rank += "B";
                    }
                    else
                    {
                        rank += "C";
                    }


                    string savedRank = PlayerPrefs.GetString(levelCode + "Rank", "");
                    int savedRankedvalue = numFromRank(savedRank);
                    if (numFromRank(rank) > savedRankedvalue)
                    {
                        PlayerPrefs.SetString(levelCode + "Rank", rank);
                        Debug.Log("Saving " + levelCode + "Rank" + " with value " + rank);
                    }
                    GetComponent<endScreen>().setEndRank(rank);

                }
                else
                {
                    Debug.Log("Please Enter A Level Code on the finishLevel Script to save completion status");
                }
                if (string.IsNullOrEmpty(nextLevelCode) == false)
                {
                    PlayerPrefs.SetInt(nextLevelCode, 1);
                }




                levelCompleteCounter = 1;
            }
            //Go ahead and reset every time
            GetComponent<endScreen>().showEndScreen();


        }


    }

    public int numFromRank(string s)
    {
        if (s.Equals("A"))
        {
            return 3;
        }
        else if (s.Equals("B"))
        {
            return 2;
        }
        else if (s.Equals("C"))
        {
            return 1;
        }


        return 0;
    }
    public bool playerCollectedItem(GameObject player)
    {
        return player.GetComponent<PlayerData>().getChallengeCollectible();
    }

    public void stopPlayerInputs(GameObject player)
    {
        player.GetComponent<playerMovementScript>().disableInputs(true);
        player.GetComponent<playerMovementScript>().zeroOutValues();
    }
}
