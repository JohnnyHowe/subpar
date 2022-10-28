using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjectOnLoad : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
