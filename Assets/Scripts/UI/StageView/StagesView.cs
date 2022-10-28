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
        foreach (Stage stage in stages)
        {
            Instantiate(stageViewPrototype, stageViewPrototype.transform.parent).Set(stage);
        }
        stageViewPrototype.gameObject.SetActive(false);
    }
}
