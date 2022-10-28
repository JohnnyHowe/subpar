using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCheck : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> objectsHit;
    public float fadeTo;

    void Update()
    {
        Vector3 direction = -(transform.position - Camera.main.transform.position).normalized;
        Debug.DrawLine(Camera.main.transform.position, transform.position, Color.white);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction);
        objectsHit = new List<GameObject>();
        for (int i = 0; i < hits.Length; i++)
        {
            objectsHit.Add(hits[i].transform.gameObject);
            Debug.DrawLine(Camera.main.transform.position, hits[i].transform.position, Color.green);
        }
    }
}
