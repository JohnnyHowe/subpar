using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LevelCompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        if (collider.TryGetComponent<Ball>(out Ball ball)) {
            GameController.Instance.OnLevelComplete();
        }
    }
}
