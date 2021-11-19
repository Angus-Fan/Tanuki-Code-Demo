using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorNode : PlayerNode
{
    //Basic node (AnchorNode)
    public AnchorNode(PlayerNode previousNode, float currentTime)
    {


        //indicator = indicatorDisplay;
        from = previousNode;
        previousNode.setToNode(this);
        time = currentTime;

    }

}
