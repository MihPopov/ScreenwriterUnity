using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string name { private set; get; }
    public string description { private set; get; }
    public Sprite icon { private set; get; }

    public InventoryItem(string name, string description, Sprite icon)
    {
        this.name = name;
        this.description = description;
        this.icon = icon;
    }
}
