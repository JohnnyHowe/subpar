using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWhenBlockingPlayer : MonoBehaviour
{
    [Range(0, 1)] float targetAlpha = 1f;
    [Range(0, 1)] float fadedAlpha = 0.5f;

    [SerializeField] LayerMask groundLayers;
    [SerializeField] float maxRayDistance = 100f;

    List<Material> fadedMaterials;

    string postfix = "FADEMESHCOPY";

    void Start()
    {
        fadedMaterials = new List<Material>();
    }

    void Update()
    {
        List<GameObject> objectsBlockingView = ObjectsBlockingView();
        List<Material> materialsBlockingView = new List<Material>();

        for (int i = 0; i < objectsBlockingView.Count; i++)
        {
            MeshRenderer meshRenderer = objectsBlockingView[i].GetComponent<MeshRenderer>();
            Material material = meshRenderer.material;
            materialsBlockingView.Add(material);
            if (!material.name.EndsWith(postfix))
            {
                meshRenderer.material = Instantiate(meshRenderer.material);
                meshRenderer.material.name += postfix;
            }
            if (!fadedMaterials.Contains(material))
            {
                fadedMaterials.Add(material);
            }
        }

        foreach (Material material in fadedMaterials)
        {
            if (materialsBlockingView.Contains(material))
            {
                SetAlpha(material, fadedAlpha);
            }
            else
            {
                SetAlpha(material, targetAlpha);
            }
        }
    }

    void SetAlpha(Material mat, float alpha)
    {
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }

    /// <summary>
    /// What objects are blocking the view to this?
    /// </summary>
    List<GameObject> ObjectsBlockingView()
    {
        Debug.DrawRay(
            transform.position,
            Camera.main.transform.position - transform.position
        );
        RaycastHit[] hits = Physics.RaycastAll(
            transform.position,
            Camera.main.transform.position - transform.position,
            maxRayDistance,
            groundLayers
            );
        List<GameObject> objects = new List<GameObject>();
        foreach (RaycastHit hit in hits)
        {
            objects.Add(hit.collider.gameObject);
        }
        return objects;
    }
}
