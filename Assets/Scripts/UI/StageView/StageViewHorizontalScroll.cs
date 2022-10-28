using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageViewHorizontalScroll : MonoBehaviour
{
    [SerializeField] private RectTransform contentContainer;
    [SerializeField] private int referenceWidth = 800;

    private int numberStages;
    private int containerStartX;

    [SerializeField] private float t;

    void Start()
    {
        numberStages = GetActiveChildren();
        containerStartX = Mathf.FloorToInt((numberStages - 1) * referenceWidth / 2f);

        SetPosition(0);
    }

    void Update()
    {
        UpdateScroll();
        SetPosition(t);
    }

    private void UpdateScroll() {
        t -= TouchInput.Instance.singleFramePositionChange.x;
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
