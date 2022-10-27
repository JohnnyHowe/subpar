using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChuteAble
{
    void SetFrozen(bool frozen);
    void SetPosition(Vector3 position);
    void SetVelocity(Vector3 velocity);
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ChuteAble : MonoBehaviour, IChuteAble
{
    private CachedMember<Rigidbody> rb;
    private CachedMember<Collider> col;

    void Awake() {
        rb = new CachedMember<Rigidbody>(GetComponent<Rigidbody>);
        col = new CachedMember<Collider>(GetComponent<Collider>);
    }

    public void SetFrozen(bool frozen)
    {
        rb.Value.isKinematic = frozen;
        col.Value.enabled = !frozen;
    }

    public void SetPosition(Vector3 position)
    {
        rb.Value.MovePosition(position);
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.Value.velocity = velocity;
    }
}
