using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private float spawnHeight = 0.24f;
    private BoxCollider _spawnArea;
    private int charactersCount;

    void Start()
    {
        charactersCount = GameObject.FindObjectOfType<Decoder>().Characters.chars.Count;
        _spawnArea = GetComponent<BoxCollider>();

        Vector3 maxSpawnPos = new Vector3(_spawnArea.size.x / 2, _spawnArea.size.y / 2, _spawnArea.size.z / 2);
        for (int i = 0; i < charactersCount; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-maxSpawnPos.x, maxSpawnPos.x), spawnHeight, UnityEngine.Random.Range(-1.9f, 1.9f));
            GameObject spawned = Instantiate(characterPrefabs[UnityEngine.Random.Range(0, characterPrefabs.Length)], Vector3.zero, Quaternion.identity);
            spawned.transform.parent = transform;
            spawned.transform.localPosition = pos;
        }
    }
}
