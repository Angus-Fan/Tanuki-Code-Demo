using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneHeadNode : PlayerNode
{
    //public GameObject cloneParent;
    public CloneHeadNode(PlayerNode previousNode, float currentTime)
    {
        //indicator = indicatorDisplay;
        from = previousNode;
        previousNode.setToNode(this);
        time = currentTime;
        //cloneParent = inputParent;
    }
}
