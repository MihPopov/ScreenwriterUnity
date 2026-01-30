using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Text infoText;
    [SerializeField] private CanvasGroup infoCanvasGroup;

    private HashSet<string> infos = new HashSet<string>();

    public void AddInfo(string info)
    {
        if (string.IsNullOrEmpty(info)) return;
        if (infos.Contains(info)) return;
        infos.Add(info);
        StartCoroutine(ShowInfo(info));
    }

    private IEnumerator ShowInfo(string info)
    {
        infoText.text = info;
        infoPanel.SetActive(true);
        yield return StartCoroutine(Fade(infoCanvasGroup, 0, 1, 0.5f));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(Fade(infoCanvasGroup, 1, 0, 0.5f));
        infoPanel.SetActive(false);
    }

    private IEnumerator Fade(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;
        cg.gameObject.SetActive(true);
        while (t < duration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }
        cg.alpha = end;
        if (end == 0f) cg.gameObject.SetActive(false);
    }

    public bool HasInfo(string info)
    {
        return infos.Contains(info);
    }
}