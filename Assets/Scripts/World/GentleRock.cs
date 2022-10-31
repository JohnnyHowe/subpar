using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleRock : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 movementAmplitude;
    [SerializeField] private Vector3 movementPeriod;
    [Header("Rotation")]
    [SerializeField] private Vector3 rotationAmplitude;
    [SerializeField] private Vector3 rotationPeriod;

    private Vector3 startLocalPosition;
    private Vector3 startLocalEulerAngles;

    private float t;

    void Start()
    {
        t = 0;
        startLocalPosition = transform.localPosition;
        startLocalEulerAngles = transform.localEulerAngles;
    }

    void Update()
    {
        t += Time.deltaTime;
        transform.localPosition = startLocalPosition + new Vector3(
            movementPeriod.x == 0? 0: F(t, movementAmplitude.x, movementPeriod.x),
            movementPeriod.y == 0? 0: F(t, movementAmplitude.y, movementPeriod.y),
            movementPeriod.z == 0? 0: F(t, movementAmplitude.z, movementPeriod.z)
        );
        transform.localEulerAngles = startLocalEulerAngles + new Vector3(
            rotationPeriod.x == 0? 0: F(t, rotationAmplitude.x, rotationPeriod.x),
            rotationPeriod.y == 0? 0: F(t, rotationAmplitude.y, rotationPeriod.y),
            rotationPeriod.z == 0? 0: F(t, rotationAmplitude.z, rotationPeriod.z)
            );
    }

    private float F(float t, float amplitude, float period)
    {
        return Mathf.Sin(2 * t * Mathf.PI / period) * amplitude;
    }
}
