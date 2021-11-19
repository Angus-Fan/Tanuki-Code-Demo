using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputRecorder : MonoBehaviour
{
    [SerializeField]

    //Timer to keepp track on when the actions have taken place
    private float timer;

    private playerMovementScript playerInputs;
    private Dictionary<float, playerInputStruct> playerInputRecord;


    void Start()
    {
        //Intialize the queue that will be used to record inputs
        playerInputRecord = new Dictionary<float, playerInputStruct>();

    }


    //Adds the timeStamp and playerInputs into the dictionary
    //The timeStamp is the key
    //The inputStruct (inputs) is the value of the key
    //This function is used by the actorObject script as the dictionary is private
    public void addToDictionary(float time, playerInputStruct inputs)
    {

        //At timestamp, if it doesn't exist in the history, add inputs
        if (!keyExists(time))
        {

            playerInputRecord.Add(time, inputs);
        }
        //If there is already an entry, then replace the inputs at key [time]
        else
        {

            playerInputRecord[time] = inputs;
        }
    }
    public void resetDictionary()
    {
        playerInputRecord = new Dictionary<float, playerInputStruct>();
    }

    public void cullExcessDictionaryKeys(float finishedTime)
    {
        Dictionary<float, playerInputStruct> shallowRecorded = new Dictionary<float, playerInputStruct>(playerInputRecord);

        foreach (KeyValuePair<float, playerInputStruct> x in playerInputRecord)
        {
            if (x.Key >= finishedTime)
            {
                shallowRecorded.Remove(x.Key);
                // Debug.Log(x.Key);
            }
        }
        playerInputRecord = shallowRecorded;
    }



    public void clearHistory()
    {
        playerInputRecord = new Dictionary<float, playerInputStruct>();
    }

    public void printDictionary()
    {
        Debug.Log(playerInputRecord.Count);
    }

    //Check if key exists
    public bool keyExists(float key)
    {
        return playerInputRecord.ContainsKey(key);
    }
    //Returns the inputStruct at current timeStamp(in)
    public playerInputStruct getRecordedInputs(float timeStamp)
    {

        return playerInputRecord[timeStamp];



    }
}
