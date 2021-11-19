using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDeath : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LevelManager levelManager;
    private IEnumerator coroutine;

    public void cloneDeath(Collider collidedObject)
    {
        //coroutine = initiateBackSequence(collidedObject);
        if (collidedObject.tag == "Player")
        {
            levelManager.back();
            //StartCoroutine(coroutine);

        }
        else if (collidedObject.tag == "Clone")
        {
            if (collidedObject.GetComponent<actorObject>().currentState == actorObject.state.Playing)
            {
                levelManager.back();
            }
            else
            {
                collidedObject.GetComponent<actorObject>().setCloneDead(true);
                collidedObject.GetComponent<actorObject>().hideClone();
            }
        }
    }

    IEnumerator initiateBackSequence(Collider collidedObject)
    {
        collidedObject.GetComponent<actorObject>().playSmokeParticle();
        collidedObject.GetComponent<actorObject>().setCloneDead(true);
        collidedObject.GetComponent<actorObject>().hideClone();
        yield return new WaitForSeconds(1);
        levelManager.back();
    }
}
