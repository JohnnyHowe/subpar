using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] private Transform steeringPlane;
    [SerializeField] private float minSteeringPlaneScale = 0.5f;
    [SerializeField] private float maxSteeringPlaneScale = 1f;
    [Header("Shoot Settings")]
    [SerializeField] float powerMultiplier = 1f;
    [SerializeField] float minPower = 1f;
    [SerializeField] float maxPower = 1f;

    Rigidbody ThisRigidBody {
        get {
            if (thisRigidBody == null) thisRigidBody = GetComponent<Rigidbody>();
            return thisRigidBody;
        }
    }
    Rigidbody thisRigidBody;

    private float steeringAngle = 0;    // relative to -z
    private float power = 0;
    private float ClampledPower {
        get => Mathf.Clamp(power, minPower, maxPower);
    }

    void Update() {
        steeringPlane.eulerAngles = Vector3.up * Mathf.LerpAngle(steeringPlane.eulerAngles.y, steeringAngle, Time.deltaTime * turnSpeed);
        steeringPlane.localScale = new Vector3(steeringPlane.localScale.x, steeringPlane.localScale.y, Mathf.Lerp(minSteeringPlaneScale, maxSteeringPlaneScale, (ClampledPower - minPower) / (maxPower - minPower)));
    }

    public void SetSteering(float angle) {
        steeringAngle = angle;
    }

    public void SetPower(float power) {
        this.power = power;
    }

    public void Shoot() {
        ThisRigidBody.AddForce(GetShootDirection() * ClampledPower * powerMultiplier);
    }

    public void ShowSteering(bool show) {
        steeringPlane.gameObject.SetActive(show);
    }

    private Vector3 GetShootDirection() {
        return Quaternion.AngleAxis(steeringAngle + 90, Vector3.up) * Vector3.right;
    }

    public void SetMoveable(bool moveable) {
        ThisRigidBody.isKinematic = !moveable;
    }

    public float GetSpeed() {
        return ThisRigidBody.velocity.magnitude;
    }
}
