using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class BreakableFloor : MonoBehaviour
{
    [SerializeField] private GameObject floorMesh;
    [SerializeField] private Image fadeImage;
    [SerializeField] private AudioSource backgroundSound;
    [SerializeField] private AudioSource crackSound;
    [SerializeField] private float fadeDuration = 2f;

    private bool triggered = false;

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        backgroundSound.mute = true;
        crackSound.volume = 1f;
        crackSound.Play();
        floorMesh.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color baseColor = fadeImage.color;
        while (t < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        SceneManager.LoadScene(4);
    }
}