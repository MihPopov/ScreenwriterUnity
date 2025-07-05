using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public int charId;
    public bool DialogOpen { get; private set; } = false;
    public GameObject dialogPanel;
    public GameObject dialogAnswerPanel;
    public Text dialogName;
    public Text dialogLine;
    public GameObject dialogAnswerPrefab;
    private Characters _chars;
    private int idx = 0;
    private int _finishedPhraseId = 0;


    private void Start()
    {
        _chars = GameObject.FindObjectOfType<Decoder>().Characters;
        _finishedPhraseId = _chars.chars[charId].data[0].id;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            dialogPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            if (DialogOpen) DialogOpen = false;
            _finishedPhraseId = idx;
            foreach (Transform c in dialogAnswerPanel.transform)
            {
                Destroy(c.gameObject); 
            }
        }
    }

    private IEnumerator ShowWindow(int id, float duration)
    {
        if (!DialogOpen) yield break;
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
        Cursor.lockState = CursorLockMode.Locked;
        if (!DialogOpen) return; 
        if (id == -2)
        {
            foreach (Transform c in dialogAnswerPanel.transform)
            {
                Destroy(c.gameObject); 
            }
            dialogPanel.SetActive(false);
            DialogOpen = false;
            _finishedPhraseId = _chars.chars[charId].data[0].id;
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
        var to = data.to[id];
        var text = to.line;
        dialogLine.text = text;
        dialogName.text = "Игрок";
        StartCoroutine(ShowWindow(int.Parse(to.id), text.Length/7f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (DialogOpen) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;
        DialogOpen = true;
        StartCoroutine(ShowWindow(_finishedPhraseId, 0));
    }
}
