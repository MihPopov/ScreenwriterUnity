using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public int charId;
    public bool DialogOpen { get; private set; } = false;
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject dialogAnswerPanel;
    [SerializeField] private Text dialogName;
    [SerializeField] private Text dialogLine;
    [SerializeField] private GameObject dialogAnswerPrefab;
    private Characters _chars;
    private int idx = 0;

    private void Start()
    {
        _chars = GameObject.FindObjectOfType<Decoder>().Characters;
    }

    private IEnumerator ShowWindow(int id, float duration)
    {
        idx = id;
        yield return new WaitForSeconds(duration);
        dialogPanel.SetActive(true);
        var character = _chars.chars[charId];
        dialogName.text = character.name;
        
        DataClass data = character.data[id];
        foreach (var d in character.data)
        {
            if (d.id == id)
            {
                data = d;
            }
        }
        dialogLine.text = data.line;
        dialogAnswerPanel.SetActive(true);
        var to = data.to;
        Cursor.lockState = CursorLockMode.None;
        if (to.Count == 0)
        {
            var obj = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
            obj.GetComponent<AnswersButtonController>().btnIdx = -2;
            obj.GetComponent<Text>().text = "Закончить диалог";
        }

        for (int i = 0; i < to.Count; i++)
        {
            var obj = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
            obj.GetComponent<AnswersButtonController>().btnIdx = i;
            obj.GetComponent<Text>().text = to[i].info;
        }
    }

    public void ButtonClicked(int id)
    {
        if (id == -2)
        {
            dialogPanel.SetActive(false);
            DialogOpen = false;
            return;
        }

        foreach (Transform c in dialogAnswerPanel.transform)
        {
            Destroy(c.gameObject); 
        }
        CharacterItem character = _chars.chars[charId];
        DataClass data = character.data[idx];
        foreach (var d in character.data)
        {
            if (d.id == idx)
            {
                data = d;
            }
        }
        dialogAnswerPanel.SetActive(false);
        print(idx.ToString());
        var to = data.to[id];
        var text = to.line;
        dialogLine.text = text;
        StartCoroutine(ShowWindow(int.Parse(to.id), text.Length/7f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (DialogOpen) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        StartCoroutine(ShowWindow(_chars.chars[charId].data[0].id, 0));
        DialogOpen = true;
    }
}
