using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAwakeEvent : MonoBehaviour
{
    [SerializeField] public UnityEvent onAwake;

    void Awake()
    {
        onAwake.Invoke();
    }
}
