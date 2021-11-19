using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ditherMesh;
    public GameObject solidMesh;
    void Start()
    {
        eventSystem.current.swapMesh += swapMesh;
    }

    public void swapMesh()
    {
        ditherMesh.SetActive(false);
        solidMesh.SetActive(true);
    }
    public void onDestroy()
    {
        eventSystem.current.swapMesh -= swapMesh;
    }
}
