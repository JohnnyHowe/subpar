using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageViewHorizontalScroll : MonoBehaviour
{
    [SerializeField] private RectTransform contentContainer;
    [SerializeField] private int referenceWidth = 800;
    [SerializeField] private float springPower = 1f;
    [SerializeField] private float springDamping = 1f;

    private int numberStages;
    private int containerStartX;
    private int targetT;

    [SerializeField] private float t;
    private float velocity;

    void Start()
    {
        numberStages = GetActiveChildren();
        containerStartX = Mathf.FloorToInt((numberStages - 1) * referenceWidth / 2f);
        targetT = 0;
        SetPosition(0);
    }

    void Update()
    {
        UpdateScroll();
        SetPosition(t);
    }

    private void UpdateScroll()
    {
        if (TouchInput.Instance.pointerHeld)
        {
            velocity = TouchInput.Instance.singleFramePositionChange.x;
        }
        else if (TouchInput.Instance.pointerUp)
        {
            float tChangeFromSpeed = Mathf.Sign(velocity);
            targetT = Mathf.Clamp(Mathf.RoundToInt(t - tChangeFromSpeed), 0, numberStages - 1);
        }
        else
        {
            float dt = targetT - t;
            velocity -= dt * Time.deltaTime * springPower;
            velocity -= velocity * springDamping;
        }
        t -= velocity;
    }

    private void SetPosition(float index)
    {
        if (numberStages > 1) index /= (numberStages - 1);
        float x = containerStartX - 2 * containerStartX * index;

        contentContainer.localPosition = new Vector3(
            x,
            contentContainer.localPosition.y,
            contentContainer.localPosition.z
            );
    }

    private int GetActiveChildren()
    {
        int count = 0;
        for (int i = 0; i < contentContainer.childCount; i++)
        {
            if (contentContainer.GetChild(i).gameObject.activeSelf) count++;
        }
        return count;
    }
}
