using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : AppSystem<LevelManager>
{
    [SerializeField] List<Stage> stages;

    public List<Stage> GetStages() {
        return stages;
    }
}
