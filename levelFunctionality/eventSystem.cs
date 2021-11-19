using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class eventSystem : MonoBehaviour
{
    public static eventSystem current;
    // Start is called before the first frame update
    private void Awake()
    {
        current = this;
    }


    public event Action<float> updateTime;
    public void updateTimeInvoked(float time)
    {
        if (updateTime != null)
        {
            updateTime(time);
        }
    }

    public event Action addResetPointPlayer;
    public void recordResetPoint()
    {
        if (addResetPointPlayer != null)
        {
            addResetPointPlayer();
        }
    }

    public event Action updateManaPanel;
    public void updateManaFunction()
    {
        if (updateManaPanel != null)
        {
            updateManaPanel();
        }
    }
    public event Action<float> returnToPoint;
    public void resetToPoint(float time)
    {
        if (returnToPoint != null)
        {
            returnToPoint(time);
        }
    }


    //I'll be using this to update UI when back is pressed
    public event Action backPressed;
    public void backButtonPressed()
    {
        if (backPressed != null)
        {
            backPressed();
        }
    }

    //I'll be using this to update UI when back is pressed
    public event Action<float> playerManaDataUpdate;
    public void updatePlayerManaData(float time)
    {
        if (playerManaDataUpdate != null)
        {
            playerManaDataUpdate(time);
        }
    }

    public event Action shiftPressed;
    public void shiftButtonPressed()
    {
        if (shiftPressed != null)
        {
            shiftPressed();
        }
    }


    public event Action swapMesh;
    public void swapMaterialToOpaque()
    {
        if (swapMesh != null)
        {
            swapMesh();
        }
    }

    public event Action levelFinishFunction;
    public void levelFinish()
    {
        if (levelFinishFunction != null)
        {
            levelFinishFunction();
        }
    }


}
