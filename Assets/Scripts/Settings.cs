using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    private void Start()
    {
        if (File.Exists(Application.dataPath + "/chars.json"))
            inputField.text = File.ReadAllText(Application.dataPath + "/chars.json");
        else
            File.Create(Application.dataPath + "/chars.json").Close();
    }

    public void Save()
    {
        File.WriteAllText(Application.dataPath + "/chars.json", inputField.text);
    }

    public void Load()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Выберите файл", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            inputField.text = File.ReadAllText(path);
        }
    }
}