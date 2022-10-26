using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMovement : MonoBehaviour
{
    [SerializeField] private Transform positionsContainer;
    [SerializeField] private Rigidbody movingObject;
    [Header("Movement Configuration")]
    [SerializeField] private bool constantVelocity = false;
    [SerializeField] private float roundTripTime = 1;
    [SerializeField] private float smoothSpeed = 0f;

    private float t;
    private List<float> consistentSpeedMultipliers;

    void Start()
    {
        t = 0;
        CalculatePath();
    }

    void CalculatePath()
    {
        CalculatePathDistances();
    }

    void FixedUpdate()
    {
        if (roundTripTime == 0) return;

        if (Debug.isDebugBuild)
        {
            CalculatePath();
        }

        t = t % positionsContainer.childCount;
        int p1t = Mathf.FloorToInt(t);
        int p2t = (p1t + 1) % positionsContainer.childCount;

        float speed;
        if (constantVelocity)
        {
            speed = consistentSpeedMultipliers[p1t] / roundTripTime;
        }
        else
        {
            speed = positionsContainer.childCount / roundTripTime;
        }
        float transformT = smoothSpeed == 0? 1: Time.deltaTime * smoothSpeed * positionsContainer.childCount;

        Vector3 p1 = positionsContainer.GetChild(p1t).position;
        Vector3 p2 = positionsContainer.GetChild(p2t).position;

        Vector3 targetPos = Vector3.Lerp(p1, p2, t % 1);
        Vector3 nextPos = Vector3.Lerp(movingObject.transform.position, targetPos, transformT);
        movingObject.MovePosition(nextPos);

        Quaternion r1 = positionsContainer.GetChild(p1t).rotation;
        Quaternion r2 = positionsContainer.GetChild(p2t).rotation;

        Quaternion targetRotation = Quaternion.Lerp(r1, r2, t % 1);
        Quaternion nextRot = Quaternion.Lerp(movingObject.transform.rotation, targetRotation, transformT);
        movingObject.MoveRotation(nextRot);

        t += Time.fixedDeltaTime * speed;
    }

    private void CalculatePathDistances()
    {
        float sum = 0;
        List<float> distances = new List<float>();
        for (int i = 0; i < positionsContainer.childCount; i++)
        {
            float currentDistance = (
                positionsContainer.GetChild(i % positionsContainer.childCount).position - 
                positionsContainer.GetChild((i + 1) % positionsContainer.childCount).position
                ).magnitude;

            distances.Add(currentDistance);
            sum += currentDistance;
        }
        List<float> speedMultipliers = new List<float>();
        for (int i = 0; i < positionsContainer.childCount; i++) {
            speedMultipliers.Add(sum / distances[i]);
        }
        consistentSpeedMultipliers = speedMultipliers;
    }
}
