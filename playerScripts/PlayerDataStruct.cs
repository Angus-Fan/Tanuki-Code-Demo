using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerDataStruct
{
    public bool itemCollected;
    public int currentMana;
    public bool canClone;
    public PlayerDataStruct(bool pickedUp, int manaValue, bool cloneEnabled)
    {
        itemCollected = pickedUp;
        currentMana = manaValue;
        canClone = cloneEnabled;
    }

}
