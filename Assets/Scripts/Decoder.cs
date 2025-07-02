using UnityEngine;

public class Decoder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/test.txt";
        StreamReader reader = new StreamReader(path);
        var str = reader.ReadToEnd());
        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
