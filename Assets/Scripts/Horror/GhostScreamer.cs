using System.Collections;
using UnityEngine;

public class GhostScreamer : MonoBehaviour
{
    [SerializeField] private GameObject screamerImage;
    [SerializeField] private float screamerDuration = 1.0f;
    [SerializeField] private AudioSource screamer;
    [SerializeField] private bool ghostIsActive = false;
    [SerializeField] private GameObject gameOverPanel;
    private bool alreadyScreamed = false;

    void OnTriggerStay(Collider other)
    {
        if (alreadyScreamed) return;
        if (other.CompareTag("Player") && ghostIsActive)
        {
            alreadyScreamed = true;
            screamer.Play();
            if (screamerImage != null) StartCoroutine(ShowScreamerImage());
        }
    }

    IEnumerator ShowScreamerImage()
    {
        foreach (Transform child in gameOverPanel.transform.parent)
        {
            child.gameObject.SetActive(false);
        }
        screamerImage.SetActive(true);
        yield return new WaitForSeconds(screamerDuration);
        screamerImage.SetActive(false);
        MenuManager menuManager = FindObjectOfType<MenuManager>();
        menuManager.Pause(gameOverPanel);
    }

    public void SetGhostActive(bool active)
    {
        ghostIsActive = active;
    }
}