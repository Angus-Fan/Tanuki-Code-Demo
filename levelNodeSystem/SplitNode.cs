using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitNode : PlayerNode
{

    public SplitNode(PlayerNode previousNode, float currentTime)
    {
        ArrayList childNodeList = new ArrayList();
        //indicator = indicatorDisplay;
        from = previousNode;
        previousNode.setToNode(this);
        time = currentTime;
    }
}
