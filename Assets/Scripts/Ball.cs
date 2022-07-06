using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private Transform steeringPlaneForward;
    [SerializeField] private Transform steeringPlaneRear;
    [SerializeField] Transform ring;
    [SerializeField] float turnSpeed = 1f;
    [SerializeField] private float minSteeringPlaneScale = 0.5f;
    [SerializeField] private float maxSteeringPlaneScale = 1f;
    [SerializeField] float ringSpinSpeed = 90f;
    [SerializeField] float arrowCenterOffset = 1f;
    [Header("Shoot Settings")]
    [SerializeField] float powerMultiplier = 1f;
    [SerializeField] float minPower = 1f;
    [SerializeField] float maxPower = 1f;

    Rigidbody ThisRigidBody
    {
        get
        {
            if (thisRigidBody == null) thisRigidBody = GetComponent<Rigidbody>();
            return thisRigidBody;
        }
    }
    Rigidbody thisRigidBody;

    private bool showingSteering = false;
    private float steeringAngle = 0;    // relative to -z
    private float power = 0;
    private float ClampledPower
    {
        get => Mathf.Clamp(power, minPower, maxPower);
    }
    bool moveable = true;

    void Update()
    {
        UpdateSteeringPlanes();

        ring.gameObject.SetActive(!moveable);
        if (!moveable) ring.transform.localEulerAngles += Vector3.up * ringSpinSpeed * Time.deltaTime;
    }

    // ============================================================================================
    // Visual Effects 
    // ============================================================================================

    private void UpdateSteeringPlanes(bool instant = false)
    {
        float scale = Mathf.Lerp(minSteeringPlaneScale, maxSteeringPlaneScale, (ClampledPower - minPower) / (maxPower - minPower));
        float t = instant ? 1 : Time.deltaTime * turnSpeed;
        float rotation = Mathf.LerpAngle(steeringPlaneForward.eulerAngles.y, steeringAngle, t);

        {
            steeringPlaneForward.eulerAngles = Vector3.up * rotation;
            steeringPlaneForward.localScale = new Vector3(steeringPlaneForward.localScale.x, steeringPlaneForward.localScale.y, scale);
            Vector3 newPos = RotatePointAroundPivot(Vector3.back * (scale * 5 + arrowCenterOffset), Vector3.zero, Vector3.up * rotation);
            newPos.y = steeringPlaneForward.localPosition.y;
            steeringPlaneForward.localPosition = newPos;
        }

        {
            steeringPlaneRear.eulerAngles = Vector3.up * rotation;
            steeringPlaneRear.localScale = new Vector3(steeringPlaneRear.localScale.x, steeringPlaneRear.localScale.y, scale);
            Vector3 newPos = RotatePointAroundPivot(Vector3.forward * (scale * 5 + arrowCenterOffset), Vector3.zero, Vector3.up * rotation);
            newPos.y = steeringPlaneForward.localPosition.y;
            steeringPlaneRear.localPosition = newPos;
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    public void ShowSteering(bool show)
    {
        if (show != showingSteering)
        {
            showingSteering = show;
            steeringPlaneForward.gameObject.SetActive(showingSteering);
            steeringPlaneRear.gameObject.SetActive(showingSteering);
            UpdateSteeringPlanes(true);
        }
    }

    public void SetSteering(float angle)
    {
        steeringAngle = angle;
    }

    public void SetPower(float power)
    {
        this.power = power;
    }

    // ============================================================================================
    // Mechanics 
    // ============================================================================================

    public void Shoot()
    {
        ThisRigidBody.AddForce(GetShootDirection() * ClampledPower * powerMultiplier);
    }

    private Vector3 GetShootDirection()
    {
        return Quaternion.AngleAxis(steeringAngle + 90, Vector3.up) * Vector3.right;
    }

    public void SetMoveable(bool moveable)
    {
        this.moveable = moveable;
        ThisRigidBody.isKinematic = !moveable;
    }

    public float GetSpeed()
    {
        return ThisRigidBody.velocity.magnitude;
    }
}
