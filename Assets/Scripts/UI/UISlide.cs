using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UISlide : MonoBehaviour
{
    RectTransform rectTransform;
    public float distanceMultiplier = 1.5f;
    public Vector2 startPosition = Vector2.left;
    Vector2 centerPosition;
    Vector2 maxOffset;
    [SerializeField] Vector2 goal;
    public float speed = 10;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        centerPosition = rectTransform.anchoredPosition;
        maxOffset = new Vector2(rectTransform.rect.width * distanceMultiplier, rectTransform.rect.height * distanceMultiplier);

        GoTo(startPosition);
    }

    private void GoTo(Vector2 destination) {
        rectTransform.anchoredPosition = centerPosition + new Vector2(maxOffset.x * destination.x, maxOffset.y * destination.y);
        goal = startPosition;
    }

    public void SlideTo(Vector2 destination)
    {
        goal = destination;
    }

    public void SlideToBottom() {
        SlideTo(Vector2.down);
    }

    public void SlideToCenter() {
        SlideTo(Vector2.zero);
    }

    public void SlideToTop() {
        SlideTo(Vector2.up);
    }

    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        Vector2 currentPosition = rectTransform.anchoredPosition;
        Vector2 destination = new Vector2(goal.x * maxOffset.x, goal.y * maxOffset.y);
        Vector2 positionChange = destination - currentPosition;
        rectTransform.anchoredPosition += positionChange * Time.unscaledDeltaTime * speed;
    }
}
