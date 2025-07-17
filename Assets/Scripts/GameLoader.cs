using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameObject mapGenerator;
    [SerializeField] private GameObject challengeMap;
    [SerializeField] private GameObject lightning;
    [SerializeField] private AudioSource common;
    [SerializeField] private AudioSource creepy;
    [SerializeField] private GameObject playerLight;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        if (PlayerPrefs.GetInt("GameMode", 0) == 0)
        {
            mapGenerator.SetActive(true);
            challengeMap.SetActive(false);
            lightning.SetActive(true);
            camera.clearFlags = CameraClearFlags.Skybox;
            creepy.mute = false;
            playerLight.SetActive(false);
        }
        else
        {
            mapGenerator.SetActive(false);
            challengeMap.SetActive(true);
            lightning.SetActive(false);
            camera.clearFlags = CameraClearFlags.SolidColor;
            common.mute = true;
        }
    }
}