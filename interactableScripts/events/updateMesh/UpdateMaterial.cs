using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateMaterial : MonoBehaviour
{
    public Material ditherMaterial;
    // Start is called before the first frame update
    void Start()
    {
        eventSystem.current.swapMesh += swapMaterial;
    }
    public void swapMaterial()
    {
        try
        {
            gameObject.GetComponent<MeshRenderer>().material = ditherMaterial;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void onDestroy()
    {
        eventSystem.current.swapMesh -= swapMaterial;
    }

}
