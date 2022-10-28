using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tymski;

[CreateAssetMenu(fileName = "New Stage", menuName = "ScriptableObjects/Stage")]
public class Stage : ScriptableObject
{
    [System.Serializable]
    public struct Level {
        public SceneReference sceneReference;
    }

    [SerializeField] public string title = "Unnamed";
    [SerializeField] public List<Level> levels;
}
