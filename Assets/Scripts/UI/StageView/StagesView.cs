using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesView : MonoBehaviour
{
    [SerializeField] private StageView stageViewPrototype;

    void Awake()
    {
        CreateView();
    }

    private void CreateView()
    {
        List<Stage> stages = LevelManager.Instance.GetStages();
        for (int i = 0; i < stages.Count; i++)
        {
            Instantiate(stageViewPrototype, stageViewPrototype.transform.parent).Set(stages[i], i + 1);
        }
        stageViewPrototype.gameObject.SetActive(false);
    }
}
