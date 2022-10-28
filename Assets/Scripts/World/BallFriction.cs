using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BallFriction : MonoBehaviour
{
    [SerializeField] private float constantDeceleration = 1f;
    [SerializeField] private float proportionalDeceleration = 1f;
    Rigidbody rigidBody;
    HashSet<Rigidbody> touchingObjects;
    Dictionary<Rigidbody, Vector3> previousVelocities;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        touchingObjects = new HashSet<Rigidbody>();
        previousVelocities = new Dictionary<Rigidbody, Vector3>();
    }

    void FixedUpdate()
    {
        Vector3 groundVelocity = Vector3.zero;
        foreach (Rigidbody rb in touchingObjects) {
            groundVelocity += rb.velocity;
        }

        Vector3 ballVelocityRelativeToGround = rigidBody.velocity - groundVelocity;
        float maxDeceleration = constantDeceleration + proportionalDeceleration * ballVelocityRelativeToGround.magnitude;
        float deceleration = Mathf.Max(maxDeceleration, ballVelocityRelativeToGround.magnitude);
        rigidBody.velocity -= deceleration * ballVelocityRelativeToGround.normalized * Time.fixedDeltaTime;

        foreach (Rigidbody rb in touchingObjects)
        {
            Vector3 acceleration = rb.velocity - previousVelocities[rb];
            rigidBody.velocity += acceleration;
            previousVelocities[rb] = rb.velocity;
        }
    }

    public Vector3 VelocityRelativeToGround() {
        Vector3 groundVelocity = Vector3.zero;
        foreach (Rigidbody rb in touchingObjects) {
            groundVelocity += rb.velocity;
        }
        return rigidBody.velocity - groundVelocity;
    }

    private Rigidbody GetFloor()
    {
        return null;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.attachedRigidbody)
        {
            if (other.collider.attachedRigidbody.isKinematic)
            {
                touchingObjects.Add(other.collider.attachedRigidbody);
                previousVelocities.Add(other.collider.attachedRigidbody, Vector3.zero);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.collider.attachedRigidbody)
        {
            touchingObjects.Remove(other.collider.attachedRigidbody);
            previousVelocities.Remove(other.collider.attachedRigidbody);
        }
    }

}
