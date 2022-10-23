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
        if (ball.Value.transform.position.y < minBallY) Restart();

        if (GameStateController.Instance.State == GameStateController.GameState.rolling)
        {

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
        }
        else
        {
            if (ball.Value.GetSpeed() > maxStillSpeed)
            {
                GameStateController.Instance.State = GameStateController.GameState.rolling;
            }
        }
    }

    void Restart()
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
}
