using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private InputField dialogField;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private InputField widthField;
    [SerializeField] private InputField heightField;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
        if (File.Exists(Application.dataPath + "/chars.json"))
            dialogField.text = File.ReadAllText(Application.dataPath + "/chars.json");
        else
            File.Create(Application.dataPath + "/chars.json").Close();
        widthField.text = PlayerPrefs.GetInt("MapWidth", 9) + "";
        heightField.text = PlayerPrefs.GetInt("MapHeight", 7) + "";
    }

    public void Save()
    {
        File.WriteAllText(Application.dataPath + "/chars.json", dialogField.text);
        PlayerPrefs.SetInt("MapWidth", int.Parse(widthField.text));
        PlayerPrefs.SetInt("MapHeight", int.Parse(heightField.text));
    }

    public void OnSoundChange(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void Load()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Выберите файл", "", "json");
        if (!string.IsNullOrEmpty(path))
        {
            dialogField.text = File.ReadAllText(path);
        }
    }
}