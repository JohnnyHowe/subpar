using System;
using UnityEngine;

public class GameSingleton<T> : MonoBehaviour
{

    public static T Instance
    {
        get
        {
            if (!instantiated)
            {
                Debug.LogError(String.Format("Trying to access GameSingleton ({}) when not yet instantiated.\nEnsure component is in scene and you're not accessing in Awake().", typeof(T).ToString()));
            }
            return instance;
        }
    }
    private static T instance;
    [SerializeField][ReadOnly] private static bool instantiated = false;

    public void Awake()
    {
        instance = GetComponent<T>();
        instantiated = true;
    }
}