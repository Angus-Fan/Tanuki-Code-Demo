using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementScript : MonoBehaviour
{

    //This class should include all the potential inpuits that the player makes
    //These two are the movement inputs in both horizontal and vertical movement
    private float horizontalValue;
    private float verticalValue;
    private bool jumpPressed;
    private bool jumpPressedUpdate;
    private bool runningPressed;
    private bool runningPressedUpdate;
    //Because the player moves in respect to the camera we need to record
    //The camera's position when recording movements
    public Transform cam;

    //we need to disable all input sometimes
    private bool inputsDisabled = false;
    public void Update()
    {
        if (!inputsDisabled)
        {
            if (Input.GetKey("space"))
            {
                jumpPressedUpdate = true;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                runningPressedUpdate = true;
            }
        }
    }

    public void listenForInput()
    {
        if (!inputsDisabled)
        {
            horizontalValue = Input.GetAxisRaw("Horizontal");
            verticalValue = Input.GetAxisRaw("Vertical");
            jumpPressed = jumpPressedUpdate;
            jumpPressedUpdate = false;
            runningPressed = runningPressedUpdate;
            runningPressedUpdate = false;
        }
    }

    public void zeroOutValues()
    {
        horizontalValue = 0;
        verticalValue = 0;
        jumpPressed = false;
        runningPressed = false;
    }

    public playerInputStruct getInputStruct()
    {
        playerInputStruct playerInputs = new playerInputStruct(horizontalValue, verticalValue, jumpPressed, runningPressed, cam.eulerAngles.y);

        return playerInputs;
    }

    public void resetInput()
    {
        horizontalValue = 0f;
        verticalValue = 0f;
    }


    public float getHVal()
    {
        return horizontalValue;
    }

    public float getVVal()
    {
        return verticalValue;
    }

    public void disableInputs(bool value)
    {
        inputsDisabled = value;
    }


}


