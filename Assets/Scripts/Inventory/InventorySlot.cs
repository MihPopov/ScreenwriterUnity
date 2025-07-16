using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public GameObject tooltipPanel;
    public Text tooltipName;
    public Text tooltipDescription;

    private InventoryItem currentItem;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        if (item != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
        else iconImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            tooltipPanel.SetActive(true);
            tooltipName.text = currentItem.name;
            tooltipDescription.text = currentItem.description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPanel.SetActive(false);
    }
}