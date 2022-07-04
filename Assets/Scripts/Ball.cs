using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float powerMultiplier = 1f;
    [SerializeField] private Transform steeringPlane;
    Rigidbody rigidBody;
    private float steeringAngle = 0;    // relative to -z
    private float power = 0;

    void Start() {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update() {
        steeringPlane.eulerAngles = Vector3.up * steeringAngle;
    }

    public void SetSteering(float angle) {
        steeringAngle = angle;
    }

    public void SetPower(float power) {
        this.power = power;
    }

    public void Shoot() {
        rigidBody.AddForce(GetShootDirection() * power * powerMultiplier);
    }

    private Vector3 GetShootDirection() {
        return Quaternion.AngleAxis(steeringAngle + 90, Vector3.up) * Vector3.right;
    }
}
