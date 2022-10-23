using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    [SerializeField] private Transform positionsContainer;
    [SerializeField] private Rigidbody movingObject;
    [SerializeField] private float speed = 1;

    private float t;

    void Start()
    {
        t = 0; 
    }

    void FixedUpdate()
    {
        t += Time.fixedDeltaTime * speed;
        t = t % positionsContainer.childCount;

        int p1t = Mathf.FloorToInt(t);
        int p2t = (p1t + 1) % positionsContainer.childCount;

        Vector3 p1 = positionsContainer.GetChild(p1t).position;
        Vector3 p2 = positionsContainer.GetChild(p2t).position;

        Vector3 pos = Vector3.Lerp(p1, p2, t % 1);
        movingObject.MovePosition(pos);
    }
}
