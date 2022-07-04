using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] Ball ball;
    [SerializeField] Collider inputRaycastPlane;
    [SerializeField] Transform originPointer;
    [SerializeField] Transform currentPointer;

    void Update()
    {
        if (TouchInput.Instance.pointerHeld)
        {
            Vector3? nStartTouch = GetPlanePosition(TouchInput.Instance.pointerStartPosition);
            Vector3? nEndTouch = GetPlanePosition(TouchInput.Instance.pointerPosition);
            if (nStartTouch == null || nEndTouch == null) return;

            Vector3 startTouch = (Vector3) nStartTouch;
            Vector3 endTouch = (Vector3) nEndTouch;

            originPointer.position = startTouch;
            currentPointer.position = endTouch;

            ball.SetSteering(Vector3.SignedAngle(Vector3.forward, (endTouch - startTouch), Vector3.up));
            ball.SetPower((startTouch - endTouch).magnitude);
        }

        if (TouchInput.Instance.pointerUp) ball.Shoot();
        ball.ShowSteering(TouchInput.Instance.pointerHeld);
        originPointer.gameObject.SetActive(TouchInput.Instance.pointerHeld);
        currentPointer.gameObject.SetActive(TouchInput.Instance.pointerHeld);
    }

    Vector3? GetPlanePosition(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(screenPosition.x * Screen.width, screenPosition.y * Screen.height));
        RaycastHit hit;
        if (inputRaycastPlane.Raycast(ray, out hit, 100.0f))
        {
            return hit.point;
        } else {
            return null;
        }
    }
}
