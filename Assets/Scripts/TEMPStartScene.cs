using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMPStartScene : MonoBehaviour
{
    public void StartScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
