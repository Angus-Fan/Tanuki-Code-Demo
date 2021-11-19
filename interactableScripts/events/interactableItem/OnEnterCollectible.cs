using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterCollectible : MonoBehaviour
{
    public GameObject manaPanelLabel;
    public GameObject manaPanelTitle;
    public GameObject instructionsPanel;

    public void itemCollect(Collider collidedObject)
    {
        //Debug.Log(collidedObject.tag);
        if (collidedObject.tag == "Player")
        {
            collidedObject.GetComponent<PlayerData>().setHasChallengeCollectible(true);
            gameObject.GetComponent<interactablePickup>().playerHasCollected();
        }


    }

    public void cloneAbilityCollect(Collider collidedObject)
    {
        //Debug.Log(collidedObject.tag);
        if (collidedObject.tag == "Player" && (collidedObject.GetComponent<actorObject>().isPlaying()))
        {
            collidedObject.GetComponent<PlayerData>().setCanClone(true);
            gameObject.GetComponent<interactablePickup>().playerHasCollected();
            enableManaHud();
            enableInstructions();
        }
        else
        {
            collidedObject.GetComponent<PlayerData>().setCanClone(true);
            gameObject.GetComponent<interactablePickup>().playerHasCollected();
            enableManaHud();
        }
    }
    public void enableInstructions()
    {
        instructionsPanel.GetComponent<ItemPickupDescription>().enableDialogue(true);
    }

    public void enableManaHud()
    {
        if (manaPanelLabel != null)
        {
            manaPanelLabel.SetActive(true);
        }
        if (manaPanelTitle != null)
        {
            manaPanelTitle.SetActive(true);
        }
    }
}
