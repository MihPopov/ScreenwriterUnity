using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public int sceneId;
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

    private List<MyScene> _scenes;
    private DialogueNode currentNode;
    private Coroutine currentCoroutine;

    private void Start()
    {
        _scenes = FindObjectOfType<Decoder>().MyScenes.scene;
        currentNode = _scenes[sceneId].data[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && dialogLayout.activeSelf)
        {
            buttonF.SetActive(false);
            dialogLayout.SetActive(false);
            buttonE.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            DialogOpen = false;

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            foreach (Transform child in dialogAnswerPanel.transform) Destroy(child.gameObject);
        }
    }

    private IEnumerator ShowWindow(int id, float delay)
    {
        if (!DialogOpen) yield break;
        yield return new WaitForSeconds(delay);
        if (!DialogOpen) yield break;
        foreach (Transform child in dialogAnswerPanel.transform) Destroy(child.gameObject);

        var scene = _scenes[sceneId];
        currentNode = scene.data.Find(n => n.id == id);

        characterPortrait.sprite = characterIcon;
        characterPortrait.gameObject.SetActive(true);
        dialogLayout.SetActive(true);
        dialogName.text = scene.npc_name;
        dialogLine.text = currentNode.line;

        dialogAnswerPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        if (currentNode.to == null || currentNode.to.Count == 0)
        {
            var btn = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
            btn.GetComponent<AnswersButtonController>().btnIdx = -2;
            btn.transform.GetChild(0).GetComponent<Text>().text = "Закончить диалог";
        }
        else
        {
            for (int i = 0; i < currentNode.to.Count; i++)
            {
                var btn = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
                btn.GetComponent<AnswersButtonController>().btnIdx = i;
                btn.transform.GetChild(0).GetComponent<Text>().text = currentNode.to[i].info;
            }
        }
    }

    public void ButtonClicked(int id)
    {
        if (!DialogOpen) return;
        foreach (Transform child in dialogAnswerPanel.transform) Destroy(child.gameObject);
        if (id == -2)
        {
            dialogLayout.SetActive(false);
            DialogOpen = false;
            buttonF.SetActive(false);
            buttonE.SetActive(true);
            currentNode = _scenes[sceneId].data[0];
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        if (id == -3)
        {
            dialogAnswerPanel.SetActive(false);
            characterPortrait.gameObject.SetActive(true);
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ShowWindow(currentNode.id, 0));
            return;
        }

        var response = currentNode.to[id];
        dialogLine.text = response.line;
        dialogName.text = _scenes[sceneId].hero_name;

        currentNode = _scenes[sceneId].data.Find(n => n.id == response.id);
        if (currentNode.goal_achieve == 1 && !inventoryManager.HasItem("Ключ"))
        {
            inventoryManager.AddItem("Ключ", "Какой-то ключ", Resources.Load<Sprite>("Textures/key"));
            inventoryManager.UpdateInventory();
        }

        var btnNext = Instantiate(dialogAnswerPrefab, dialogAnswerPanel.transform);
        btnNext.GetComponent<AnswersButtonController>().btnIdx = -3;
        btnNext.transform.GetChild(0).GetComponent<Text>().text = "Продолжить диалог";
        dialogAnswerPanel.SetActive(true);
        dialogAnswerPanel.SetActive(true);
        characterPortrait.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || DialogOpen) return;
        if (Input.GetKey(KeyCode.E))
        {
            DialogOpen = true;
            buttonE.SetActive(false);
            buttonF.SetActive(true);
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(ShowWindow(currentNode.id, 0));
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