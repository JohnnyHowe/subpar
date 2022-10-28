using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFade : MonoBehaviour
{
    public float fadeSpeed = 10f;
    public float fadeTo = 1f;
    public float fadeFrom = 0.51f;

    FadeCheck fadeCheck;
    Renderer objectRenderer;

    Material uniqueMaterial;

    // Start is called before the first frame update
    void Start()
    {
        fadeCheck = GameObject.Find("Player").GetComponent<FadeCheck>();

        objectRenderer = GetComponent<Renderer>();
        uniqueMaterial = TransparentCopy(objectRenderer.material);
        objectRenderer.material = uniqueMaterial;
    }

    Material TransparentCopy(Material toCopy) {
        Material material = Instantiate(toCopy);
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.SetFloat("_Mode", 3);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        return material;
    }

    void Update()
    {
        float targetAlpha = ShouldFade()? fadeFrom : fadeTo;
        float nextAlpha = Mathf.Lerp(objectRenderer.material.color.a, targetAlpha, Mathf.Min(1, Time.deltaTime * fadeSpeed));
        SetAlpha(objectRenderer.material, nextAlpha);
    }

    bool ShouldFade() {
        return fadeCheck.objectsHit.Contains(gameObject);
    }

    void SetAlpha(Material mat, float alpha) {
        Color c = mat.color;
        c.a = alpha;
        mat.color = c;
    }
}
