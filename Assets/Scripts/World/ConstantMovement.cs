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
    [SerializeField] private float resetTime = 0f;
    [Header("Debug Bits (All Optional)")]
    [SerializeField] private Transform targetTransform;

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

        float speed = 1;

        if (t < 1) {
            int lastNodeIndex = Mathf.FloorToInt(t * positionsContainer.childCount);
            float speedVelocitySmoothingFactor = constantVelocity ? consistentSpeedMultipliers[lastNodeIndex] : 1;
            speed = 1f / roundTripTime * speedVelocitySmoothingFactor;
        }

        MoveObject(Mathf.Min(t, 1), smoothSpeed == 0 ? 1 : Time.deltaTime * smoothSpeed * positionsContainer.childCount);

        t += Time.fixedDeltaTime * speed;
        t = t % (1 + resetTime);
    }

    /// <summary>
    /// Move the object to a position and rotation based on t
    /// 0 <= t < 1
    /// </summary>
    private void MoveObject(float t, float smoothT)
    {
        t = Mathf.Min(t, 0.9999999f);

        float fullT = t * positionsContainer.childCount;
        float nodeLevelT = fullT % 1;

        int p1t = Mathf.FloorToInt(fullT);
        int p2t = (p1t + 1) % positionsContainer.childCount;

        Vector3 p1 = positionsContainer.GetChild(p1t).position;
        Vector3 p2 = positionsContainer.GetChild(p2t).position;

        Vector3 targetPos = Vector3.Lerp(p1, p2, nodeLevelT);
        Vector3 nextPos = Vector3.Lerp(movingObject.transform.position, targetPos, smoothT);
        movingObject.MovePosition(nextPos);

        Quaternion r1 = positionsContainer.GetChild(p1t).rotation;
        Quaternion r2 = positionsContainer.GetChild(p2t).rotation;

        Quaternion targetRotation = Quaternion.Lerp(r1, r2, nodeLevelT);
        Quaternion nextRot = Quaternion.Lerp(movingObject.transform.rotation, targetRotation, smoothT);
        movingObject.MoveRotation(nextRot);

        if (targetTransform) 
        {
            Debug.DrawLine(movingObject.position, targetPos, Color.white);
            Debug.DrawLine(targetTransform.position, p1, Color.white);
            Debug.DrawLine(targetTransform.position, p2, Color.white);

            targetTransform.rotation = targetRotation;
            targetTransform.position = targetPos;
        }
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
        for (int i = 0; i < positionsContainer.childCount; i++)
        {
            speedMultipliers.Add(sum / (distances[i] * positionsContainer.childCount));
        }
        consistentSpeedMultipliers = speedMultipliers;
    }
}
