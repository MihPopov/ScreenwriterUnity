using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(BoxCollider))]
public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Sprite[] characterIcons;
    [SerializeField] private float spawnHeight = -1.1f;
    [SerializeField] private float minDistanceCTC = 10f;
    [SerializeField] private float checkRadius = 5f;

    [Header("Dialog UI")]
    [SerializeField] private GameObject dialogLayout;
    [SerializeField] private GameObject dialogAnswerPanel;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Text dialogName;
    [SerializeField] private Text dialogLine;
    [SerializeField] private GameObject dialogAnswerPrefab;
    [SerializeField] private GameObject buttonE;
    [SerializeField] private GameObject buttonF;
    [SerializedDictionary("ItemID", "InventoryItem")]
    [SerializeField] private SerializedDictionary<int, InventoryItem> items;

    private BoxCollider _spawnArea;
    private int _sceneCount;
    private List<Vector3> characterPositions = new List<Vector3>();

    void Start()
    {
        List<MyScene> scenes = FindObjectOfType<Decoder>().MyScenes.scene;
        _sceneCount = scenes.Count;
        _spawnArea = GetComponent<BoxCollider>();
        Vector3 maxSpawnPos = new Vector3(_spawnArea.size.x / 2, _spawnArea.size.y / 2, _spawnArea.size.z / 2);

        for (int i = 0; i < _sceneCount; i++)
        {
            int prefabIndex = Random.Range(0, characterPrefabs.Length);
            Vector3 characterPos;
            bool validPosition;
            do
            {
                characterPos = new Vector3(
                    Random.Range(-maxSpawnPos.x, maxSpawnPos.x),
                    spawnHeight,
                    Random.Range(-maxSpawnPos.z, maxSpawnPos.z)
                );
                bool noBuildingCollision = true;
                Collider[] hitColliders = Physics.OverlapSphere(characterPos, checkRadius);
                foreach (var hit in hitColliders)
                {
                    if (hit.CompareTag("Building"))
                    {
                        noBuildingCollision = false;
                        break;
                    }
                }
                validPosition = characterPositions.TrueForAll(existing =>
                {
                    float distance = Vector2.Distance(
                        new Vector2(characterPos.x, characterPos.z),
                        new Vector2(existing.x, existing.z)
                    );
                    return distance >= minDistanceCTC / _sceneCount;
                }) && noBuildingCollision;

            } while (!validPosition);

            characterPositions.Add(characterPos);
            GameObject spawned = Instantiate(characterPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);
            spawned.transform.parent = transform;
            spawned.transform.localPosition = characterPos;
            DialogController dc = spawned.GetComponent<DialogController>();
            dc.sceneId = i;
            dc.dialogLayout = dialogLayout;
            dc.dialogAnswerPanel = dialogAnswerPanel;
            dc.dialogLine = dialogLine;
            dc.dialogAnswerPrefab = dialogAnswerPrefab;
            dc.dialogName = dialogName;
            dc.characterPortrait = characterPortrait;
            dc.characterIcon = characterIcons[prefabIndex];
            dc.buttonE = buttonE;
            dc.buttonF = buttonF;
            dc.items = items;
            spawned.transform.Find("Point").Find("Text").GetComponent<TMP_Text>().text = (i + 1).ToString();
        }
    }
}