using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InteractablePickupStruct
{
    // Update is called once per frame
    public bool itemCollected;

    public InteractablePickupStruct(bool pickedUp)
    {
        itemCollected = pickedUp;
    }
}
