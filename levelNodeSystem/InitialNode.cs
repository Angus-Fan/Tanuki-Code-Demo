using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InitialNode : PlayerNode
{
    //The initial node
    public InitialNode(float initialTime, GameObject indicatorDisplay)
    {
        indicator = indicatorDisplay;
        time = initialTime;
        origin = true;

    }
}
