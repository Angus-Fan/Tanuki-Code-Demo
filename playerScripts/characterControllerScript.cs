using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class characterControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController cc;
    public GameObject cam;
    float camAngle;
    public float moveSpeed;
    public float runSpeed;
    public float grav = 9.81f;
    public float heavyGrav = 10;
    public float jumpSpeed = 5.5f;
    private float tempJump;
    public bool isSelected;
    public bool variableJump;
    private float timeJump;
    public bool isGrounded;


    private Animator characterAnimator;
    private int movementHash;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        cc.enableOverlapRecovery = true;
        Physics.IgnoreLayerCollision(9, 12);
        isSelected = true;
        variableJump = true;
        try
        {
            characterAnimator = GetComponentInChildren<Animator>();
            movementHash = Animator.StringToHash("velocity");
        }
        catch (Exception e)
        {
            Debug.Log("Couldn't find the animator");
            Debug.Log(e);
        }
    }

    public void Update()
    {
        isGrounded = cc.isGrounded;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            variableJump = !variableJump;
        }
    }



    // Update is called once per frame
    public void toggleCharacterController(bool value)
    {
        cc.enabled = value;
    }
    public void move(playerInputStruct playerInputs)
    {
        float speed = moveSpeed;
        camAngle = playerInputs.cameraAngleYInput;
        //float horizontalValue = Input.GetAxisRaw("Horizontal");
        //float verticalValue = Input.GetAxisRaw("Vertical");
        float horizontalValue = playerInputs.horizontalInput;
        float verticalValue = playerInputs.verticalInput;
        Vector3 movement = new Vector3(horizontalValue, 0, verticalValue);
        if (movement.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + camAngle;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            movement = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        if (playerInputs.jumped && cc.isGrounded)
        {
            tempJump = jumpSpeed;
            timeJump = Time.time;
            // jumpedHori = horizontalValue;
            // jumpedVerti = verticalValue;
        }
        if (!cc.isGrounded)
        {
            if (variableJump)
            {
                if (!(playerInputs.jumped && Time.time - timeJump < 0.25))
                {
                    tempJump -= grav * Time.deltaTime;
                }
            }
            else
            {
                tempJump -= grav * Time.deltaTime;
            }
        }
        if (playerInputs.running)
        {
            speed = runSpeed;
        }
        movement.y = tempJump;
        setAnimationValues(movement, speed, tempJump);
        cc.Move(movement * speed * Time.deltaTime);
    }

    public void setAnimationValues(Vector3 inputs, float currentSpeed, float jump)
    {
        Vector2 movementXY = new Vector2(inputs.x, inputs.z);
        float magnitude = movementXY.magnitude;

        if (currentSpeed == moveSpeed && magnitude != 0)
        {
            characterAnimator.SetInteger("velocity", 1);
        }
        else if (currentSpeed == runSpeed && magnitude != 0)
        {
            characterAnimator.SetInteger("velocity", 2);
        }
        else
        {
            characterAnimator.SetInteger("velocity", 0);
        }


        if (cc.isGrounded)
        {
            characterAnimator.SetInteger("jump", 0);

        }
        else if (jump == jumpSpeed)
        {
            characterAnimator.SetInteger("jump", 1);
        }


    }

    public void setIdle()
    {
        characterAnimator.SetInteger("velocity", 0);
        characterAnimator.SetInteger("jump", 0);
    }



    public void emptyMove()
    {
        characterAnimator.SetInteger("velocity", 0);

        Vector3 movement = new Vector3(0, 0, 0);
        if (!cc.isGrounded)
        {
            tempJump -= grav * Time.deltaTime;
        }
        movement.y = tempJump;
        //You'll only have a parent if you're on a platform
        if (gameObject.transform.parent != null)
        {
            if (movement.x == 0 && movement.y == 0 || !cc.isGrounded)
            {
                cc.Move(movement * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            cc.Move(movement * moveSpeed * Time.deltaTime);
        }

    }



    public void resetVals()
    {
        tempJump = 0;
        cc.Move(Vector3.zero * Time.deltaTime);
    }

    public float getTempJumpSpeed()
    {
        return tempJump;
    }

    public CharacterController getCharacterCont()
    {
        return cc;
    }

    public void resetToTheseValues(float jumpSpeed)
    {
        tempJump = jumpSpeed;
    }
    // this script pushes all rigidbodies that the character touches
    float pushPower = 5.0f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;



    }
}
