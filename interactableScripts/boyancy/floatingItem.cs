using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingItem : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged;
    public float displacementAmount;
    public float waterLevel;
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < waterLevel)
        {

            float displacementMultiplier = Mathf.Clamp01((waterLevel - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), ForceMode.Acceleration);
        }
    }
}
