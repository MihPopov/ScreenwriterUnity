using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public int charId;
    public bool DialogOpen { get; private set; } = false;
    public GameObject dialogLayout;
    public GameObject dialogAnswerPanel;
    public Image characterPortrait;
    public Text dialogName;
    public Text dialogLine;
    public Sprite characterIcon; 
    public GameObject dialogAnswerPrefab;
    public GameObject buttonE;
    public GameObject buttonF;
    public InventoryManager inventoryManager;
    private Characters _chars;
    private int idx = 0;
    private int _finishedPhraseId = 0;
    private Coroutine currentCoroutine;

    private void Start()
    {
        _chars = FindObjectOfType<Decoder>().Characters;
        _finishedPhraseId = _chars.chars[charId].data[0].id;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && dialogLayout.activeSelf)
        {
            buttonF.SetActive(false);
            dialogLayout.SetActive(false);
            buttonE.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            if (DialogOpen) DialogOpen = false;
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
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

        if (!DialogOpen) yield break;

        foreach (Transform c in dialogAnswerPanel.transform)
        {
            Destroy(c.gameObject);
        }

        characterPortrait.sprite = characterIcon;
        dialogLayout.SetActive(true);
        characterPortrait.gameObject.SetActive(true);
        var character = _chars.chars[charId];
        dialogName.text = character.name;
        DataClass data = character.data.Find(d => d.id == id);
        dialogLine.text = data.line;

        dialogAnswerPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        if (data.to.Count == 0)
        {
            var obj = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
            obj.GetComponent<AnswersButtonController>().btnIdx = -2;
            obj.transform.GetChild(0).GetComponent<Text>().text = "Закончить диалог";
        }
        else
        {
            for (int i = 0; i < data.to.Count; i++)
            {
                var obj = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
                obj.GetComponent<AnswersButtonController>().btnIdx = i;
                obj.transform.GetChild(0).GetComponent<Text>().text = data.to[i].info;
            }
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
            dialogLayout.SetActive(false);
            DialogOpen = false;
            buttonF.SetActive(false);
            buttonE.SetActive(true);
            inventoryManager.AddItem("Ключ", "Какой-то ключ", Resources.Load<Sprite>("Textures/key"));
            inventoryManager.UpdateInventory();
            _finishedPhraseId = _chars.chars[charId].data[0].id;
            return;
        }

        foreach (Transform c in dialogAnswerPanel.transform)
        {
            Destroy(c.gameObject);
        }

        CharacterItem character = _chars.chars[charId];
        DataClass data = character.data.Find(d => d.id == idx);

        dialogAnswerPanel.SetActive(false);
        characterPortrait.gameObject.SetActive(false);
        var to = data.to[id];
        dialogLine.text = to.line;
        dialogName.text = "Игрок";

        _finishedPhraseId = int.Parse(to.id);
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ShowWindow(_finishedPhraseId, to.line.Length / 10f));
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (DialogOpen) return;
        if (Input.GetKey(KeyCode.E))
        {
            DialogOpen = true;
            buttonE.SetActive(false);
            buttonF.SetActive(true);
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ShowWindow(_finishedPhraseId, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogLayout.activeSelf) buttonE.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) buttonE.SetActive(false);
    }
}