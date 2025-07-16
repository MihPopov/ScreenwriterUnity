using System.IO;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    public MyScenes MyScenes { get; private set; }
    
    private void Awake()
    {
        string path = Application.dataPath + "/scene.json";
        StreamReader reader = new StreamReader(path);
        var str = reader.ReadToEnd();
        reader.Close();
        MyScenes = JsonUtility.FromJson<MyScenes>("{\"scene\":" + str + "}");
    }
}