using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : AppSystem<LevelManager>
{
    [SerializeField] List<Stage> stages;

    [SerializeField][ReadOnly] private int currentStageIndex;
    [SerializeField][ReadOnly] private int currentLevelIndex;

    void Start()
    {
        LoadIndiciesFromActiveScene();

        SceneManager.activeSceneChanged += (oldScene, newScene) => {
            LoadIndiciesFromActiveScene();
        };
    }

    /// <summary>
    /// Using the active scene, set currentStageIndex and currentLevelIndex
    /// </summary>
    private void LoadIndiciesFromActiveScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        for (int stageIndex = 0; stageIndex < stages.Count; stageIndex++)
        {
            for (int levelIndex = 0; levelIndex < stages[stageIndex].levels.Count; levelIndex++)
            {
                if (stages[stageIndex].levels[levelIndex].sceneReference.SceneName == currentSceneName)
                {
                    currentStageIndex = stageIndex;
                    currentLevelIndex = levelIndex;
                    return;
                }
            }
        }
        currentStageIndex = 0;
        currentLevelIndex = -1;
        throw new System.Exception(string.Format("Could not find current level from scene \"{0}\"", currentSceneName));
    }

    public List<Stage> GetStages()
    {
        return stages;
    }

    private void StartLevel(Stage.Level level)
    {
        SceneManager.LoadScene(level.sceneReference);
    }

    public Stage GetCurrentStage()
    {
        return stages[currentStageIndex];
    }

    public Stage.Level GetCurrentLevel()
    {
        return GetCurrentStage().levels[currentLevelIndex];
    }

    public void StartNextLevel()
    {
        // TODO what if current index bad?
        IncreaseCurrentLevelIndex();
        StartLevel(GetCurrentLevel());
    }


    private void IncreaseCurrentLevelIndex()
    {
        // TODO what if current index bad?
        currentLevelIndex += 1;
        if (currentLevelIndex >= GetCurrentStage().levels.Count && currentStageIndex < stages.Count - 1)
        {
            currentLevelIndex = 0;
            currentStageIndex += 1;
        }
    }
}
