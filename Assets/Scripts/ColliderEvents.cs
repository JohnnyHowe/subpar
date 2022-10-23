using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Collider))]
public class ColliderEvents : MonoBehaviour
{
    [System.Serializable]
    public class ColliderEvent : UnityEvent<Collider> { }
    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }

    [SerializeField] public ColliderEvent onTriggerEnter;
    [SerializeField] public ColliderEvent onTriggerExit;
    [SerializeField] public CollisionEvent onCollisionEnter;
    [SerializeField] public CollisionEvent onCollisionExit;

    void OnTriggerEnter(Collider collider)
    {
        onTriggerEnter.Invoke(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        onTriggerExit.Invoke(collider);
    }

    void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        onCollisionExit.Invoke(collision);
    }
}
