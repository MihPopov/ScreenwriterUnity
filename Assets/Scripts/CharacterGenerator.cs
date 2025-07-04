using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private float spawnHeight = 0.24f;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject dialogAnswerPanel;
    [SerializeField] private Text dialogName;
    [SerializeField] private Text dialogLine;
    [SerializeField] private GameObject dialogAnswerPrefab;
    private BoxCollider _spawnArea;
    private int _charactersCount;

    void Start()
    {
        _charactersCount = GameObject.FindObjectOfType<Decoder>().Characters.chars.Count;
        _spawnArea = GetComponent<BoxCollider>();

        Vector3 maxSpawnPos = new Vector3(_spawnArea.size.x / 2, _spawnArea.size.y / 2, _spawnArea.size.z / 2);
        for (int i = 0; i < _charactersCount; i++)
        {
            Vector3 pos = new Vector3(UnityEngine.Random.Range(-maxSpawnPos.x, maxSpawnPos.x), spawnHeight, UnityEngine.Random.Range(-1.9f, 1.9f));
            GameObject spawned = Instantiate(characterPrefabs[UnityEngine.Random.Range(0, characterPrefabs.Length)], Vector3.zero, Quaternion.identity);
            spawned.transform.parent = transform;
            spawned.transform.localPosition = pos;
            DialogController dc = spawned.GetComponent<DialogController>();
            dc.charId = i;
            dc.dialogPanel = dialogPanel;
            dc.dialogAnswerPanel = dialogAnswerPanel;
            dc.dialogLine = dialogLine;
            dc.dialogAnswerPrefab = dialogAnswerPrefab;
            dc.dialogName = dialogName;
            spawned.transform.Find("Point").Find("Text").GetComponent<TMP_Text>().text = (i+1).ToString();  
        }
    }
}
