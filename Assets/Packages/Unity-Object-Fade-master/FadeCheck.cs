using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCheck : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> objectsHit;
    [HideInInspector]
    public GameObject parentObjectHit;
    [Range(0.0f, 1.0f)]
    public float fadeTo;

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = -(transform.position - Camera.main.transform.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction);
        objectsHit = new List<GameObject>();
        for (int i = 0; i < hits.Length; i++)
        {
            objectsHit.Add(hits[i].transform.gameObject);
            // GameObject findParent = hit.transform.gameObject;
            // while (findParent.transform.parent != null)
            // {
            //     findParent = findParent.transform.parent.gameObject;
            // }
            // parentObjectHit = findParent;

            // Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.green);
        }
        // Debug.DrawRay(Camera.main.transform.position, direction * 50);
    }
}
