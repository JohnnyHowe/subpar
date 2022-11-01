using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private UISlide container;

    private void Start() {
        GameStateController.Instance.onPostGameStart.AddListener(Show);
    }

    private void Show() {
        container.SlideToCenter();
    }

    public void GoToNextLevel() {
        LevelManager.Instance.StartNextLevel();   
    }

    public void RetryCurrentLevel() {
        GameController.Instance.Restart();
    }
}
