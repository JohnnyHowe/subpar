using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartup
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad() {
        Application.targetFrameRate = Screen.resolutions[0].refreshRate;
        Time.fixedDeltaTime = 1f / Application.targetFrameRate;
        Debug.Log("Target Refresh Rate: " + Application.targetFrameRate);
    }
}
