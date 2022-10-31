using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : GameSingleton<GameController>
{
    [SerializeField] CachedMember<Ball> ball;
    [SerializeField] float minBallY = -1f;
    [SerializeField] float maxStillSpeed = 0.1f;
    [SerializeField] float minStillTime = 0.5f;
    float currentStillTime = 0f;

    void Start()
    {
        ball = new CachedMember<Ball>(GameObject.FindGameObjectWithTag("Player").GetComponent<Ball>);

        GameStateController.Instance.onAimingStart.AddListener(OnAimStart);
        GameStateController.Instance.onRollingStart.AddListener(OnRollingStart);
    }

    void FixedUpdate()
    {
        switch (GameStateController.Instance.State)
        {
            case GameStateController.GameState.aiming:
                if (ball.Value.GetSpeed() > maxStillSpeed)
                {
                    GameStateController.Instance.State = GameStateController.GameState.rolling;
                }
                break;
            case GameStateController.GameState.rolling:
                if (ball.Value.transform.position.y < minBallY) Restart();
                if (ball.Value.GetSpeed() < maxStillSpeed)
                {
                    currentStillTime += Time.deltaTime;
                }
                else
                {
                    currentStillTime = 0;
                }

                if (currentStillTime > minStillTime)
                {
                    currentStillTime = 0;
                    GameStateController.Instance.State = GameStateController.GameState.aiming;
                }
                break;
            case GameStateController.GameState.postGame:
                break;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnAimStart()
    {
        // inputController.enabled = true;
        ball.Value.SetMoveable(false);
    }

    void OnRollingStart()
    {
        // inputController.enabled = false;
        ball.Value.SetMoveable(true);
    }

    public void OnLevelComplete()
    {
        GameStateController.Instance.State = GameStateController.GameState.postGame;
    }
}
