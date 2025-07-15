using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemReceivePanel;
    [SerializeField] private Text itemName;
    [SerializeField] private Image itemIcon;
    [SerializeField] private CanvasGroup itemPanelCanvasGroup;
    public InventorySlot[] slots;
    public List<InventoryItem> items;

    void Start()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count) slots[i].SetItem(items[i]);
            else slots[i].SetItem(null);
        }
    }

    public void AddItem(string name, string description, Sprite icon)
    {
        InventoryItem newItem = new InventoryItem(name, description, icon);
        items.Add(newItem);
        UpdateInventory();
        StartCoroutine(ShowItemReceivePanel(name, icon));
    }

    private IEnumerator ShowItemReceivePanel(string name, Sprite icon)
    {
        itemName.text = name;
        itemIcon.sprite = icon;
        itemReceivePanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(itemPanelCanvasGroup, 0, 1, 0.5f));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeCanvasGroup(itemPanelCanvasGroup, 1, 0, 0.5f));
        itemReceivePanel.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;
        cg.gameObject.SetActive(true);
        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        cg.alpha = end;
        if (end == 0f) cg.gameObject.SetActive(false);
    }

    public bool HasItem(string itemName)
    {
        foreach (var item in items)
        {
            if (item.name == itemName) return true;
        }
        return false;
    }
}