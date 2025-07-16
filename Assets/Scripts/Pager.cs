using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pager : MonoBehaviour
{
    [SerializeField] private GameObject from;
    [SerializeField] private GameObject to;

    public void OpenPage()
    {
        to.SetActive(true);
        from.SetActive(false);
    }
}
