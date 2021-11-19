using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerNode
{

    public PlayerNode from = null;
    public PlayerNode to = null;
    public PlayerNode split = null;
    public float time = 0;
    public bool origin = false;
    public GameObject indicator;
    public GameObject nodeCloneHolder = null;

    public PlayerNode()
    {


    }

    public void setIndicator(GameObject input)
    {
        indicator = input;
    }


    public void setSplitNode(PlayerNode toNode)
    {
        split = toNode;
    }
    public void setToNode(PlayerNode toNode)
    {
        to = toNode;
    }

    public void setFromNode(PlayerNode fromNode)
    {
        from = fromNode;
    }
    public void debugNode()
    {
        Debug.Log("The currentNode is from : " + from);
        Debug.Log("The currentNode is to : " + to);
        Debug.Log("The currentNode is a split to: " + split);
        Debug.Log("The currentNode time is : " + time);
        Debug.Log("The currentNode time is origin? : " + origin);
    }
    public void setCloneField(GameObject clone)
    {
        nodeCloneHolder = clone;
    }
    public GameObject getCloneField()
    {
        return nodeCloneHolder;
    }
}
