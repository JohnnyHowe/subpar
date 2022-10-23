using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute : MonoBehaviour
{
    [SerializeField] private Chute nextChute;
    [SerializeField] private ColliderEvents trigger;
    [SerializeField] private Vector3 exitVelocity = Vector3.up;

    [SerializeField] private List<GameObject> doNotTeleport;

    void Awake()
    {
        doNotTeleport = new List<GameObject>();
        trigger.onTriggerEnter.AddListener((c) => OnTriggerEnter(c));
        trigger.onTriggerExit.AddListener((c) => OnTriggerExit(c));
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!doNotTeleport.Contains(collider.transform.gameObject))
        {
            nextChute.TeleportTo(collider.transform);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        doNotTeleport.Remove(collider.transform.gameObject);
    }

    public void TeleportTo(Transform objectTransform)
    {
        if (!doNotTeleport.Contains(objectTransform.gameObject))
        {
            doNotTeleport.Add(objectTransform.gameObject);
            objectTransform.position = transform.position;
            objectTransform.GetComponent<Rigidbody>().velocity = exitVelocity;
        }
    }
}
