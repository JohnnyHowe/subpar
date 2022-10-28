using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class StageViewLevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Transform starContainer;
    private UnityAction pressedCallback;

    public void Set(int number, int nStars, bool completed, UnityAction pressedCallback) {
        numberText.text = number.ToString();
        this.pressedCallback = pressedCallback;

        for (int i = 0; i < starContainer.childCount; i++) {
            starContainer.GetChild(i).gameObject.SetActive(i < nStars);
        }
    }

    public void PressedCallback() {
        pressedCallback.Invoke();
    }
}