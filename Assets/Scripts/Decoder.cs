using System.IO;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    public Characters Characters { get; private set; }
    
    private void Start()
    {
        string path = Application.dataPath + "/chars.json";
        StreamReader reader = new StreamReader(path);
        var str = reader.ReadToEnd();
        reader.Close();
        Characters = JsonUtility.FromJson<Characters>("{\"chars\":" + str + "}");
    }
}