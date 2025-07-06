using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        Cursor.lockState = pausePanel.activeSelf ? CursorLockMode.Locked : CursorLockMode.None;
        Time.timeScale = pausePanel.activeSelf ? 1 : 0;
        pausePanel.SetActive(!pausePanel.activeSelf);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1;
    }
}