using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private GameObject mapGenerator;
    [SerializeField] private GameObject challengeMap;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("GameMode", 0) == 0)
        {
            mapGenerator.SetActive(true);
            challengeMap.SetActive(false);
        }
        else
        {
            mapGenerator.SetActive(false);
            challengeMap.SetActive(true);
        }
    }
}
