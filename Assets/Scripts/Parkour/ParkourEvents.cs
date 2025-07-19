using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParkourEvents : MonoBehaviour
{
    [SerializeField] private GameObject targetGroup;
    [SerializeField] private float fadeDuration = 2f;

    private Dictionary<Renderer, Material[]> fadeMaterialMap = new Dictionary<Renderer, Material[]>();

    public void ShowPart2()
    {
        fadeMaterialMap.Clear();
        foreach (Renderer r in targetGroup.GetComponentsInChildren<Renderer>(true))
        {
            Material[] newMats = new Material[r.materials.Length];
            for (int i = 0; i < r.materials.Length; i++)
            {
                Material original = r.materials[i];
                Material fadeMat = new Material(original);
                fadeMat.SetFloat("_Mode", 2);
                fadeMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                fadeMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                fadeMat.SetInt("_ZWrite", 0);
                fadeMat.DisableKeyword("_ALPHATEST_ON");
                fadeMat.EnableKeyword("_ALPHABLEND_ON");
                fadeMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                fadeMat.renderQueue = 3000;
                Color c = fadeMat.color;
                c.a = 0f;
                fadeMat.color = c;
                newMats[i] = fadeMat;
            }
            r.materials = newMats;
            fadeMaterialMap.Add(r, newMats);
        }
        StartCoroutine(FadeInAfterDelay());
    }

    private IEnumerator FadeInAfterDelay()
    {
        targetGroup.SetActive(true);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            foreach (var kvp in fadeMaterialMap)
            {
                Material[] materials = kvp.Value;
                foreach (Material mat in materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color c = mat.color;
                        c.a = Mathf.Lerp(0f, 1f, t);
                        mat.color = c;
                    }
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var kvp in fadeMaterialMap)
        {
            foreach (Material mat in kvp.Value)
            {
                if (mat.HasProperty("_Color"))
                {
                    Color c = mat.color;
                    c.a = 1f;
                    mat.color = c;
                }
            }
        }
    }
}