using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject dialogAnswerPanel;
    [SerializeField] private Text dialogName;
    [SerializeField] private Text dialogLine;
    [SerializeField] private GameObject dialogAnswerPrefab;
    private AnswersButtonsController _answers = null;
    private Characters _chars;
    private bool _isStarted = false;
    private int _idx;

    private void Start()
    {
        _chars = GameObject.FindObjectOfType<Decoder>().Characters;
    }

    private void Update()
    {
        if (!_answers) return;
        if (_answers.btnIdx != -1)
        {
            
        }
    }

    private void ShowWindow(int charId, int id)
    {
        dialogPanel.SetActive(true);
        var character = _chars.chars[charId];
        dialogName.text = character.name;
        dialogLine.text = character.data[id].line;
        dialogAnswerPanel.SetActive(true);
        var to = character.data[id].to;
        foreach (var t in to)
        {
            var obj = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
            obj.transform.Find("Text").GetComponent<Text>().text = t.info;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isStarted) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowWindow(0, 0);
            _isStarted = true;
        }
    }
}
