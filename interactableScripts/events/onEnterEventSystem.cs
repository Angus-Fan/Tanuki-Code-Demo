using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class onEnterEventSystem : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> onEnterTrigger;
    [SerializeField] private UnityEvent<Collider> onExitTrigger;
    [SerializeField] private UnityEvent<Collider> onStayTrigger;

    private void OnTriggerEnter(Collider collidedObject)
    {
        onEnterTrigger.Invoke(collidedObject);
    }
    private void OnTriggerExit(Collider collidedObject)
    {
        onExitTrigger.Invoke(collidedObject);
    }
    private void OnTriggerStay(Collider collidedObject)
    {
        onStayTrigger.Invoke(collidedObject);
    }
}
