using System.IO;
using UnityEngine;

public class Decoder : MonoBehaviour
{
    private void Start()
    {
        string path = Application.dataPath + "/chars.json";
        StreamReader reader = new StreamReader(path);
        var str = reader.ReadToEnd();
        print(str);
        reader.Close();
        CharacterClass user = JsonUtility.FromJson<CharacterClass>(str);
        print("Id " + user.Id);
        print("Name " + user.Name);
        print(user.Data);
        foreach (var d in user.Data)
        {
            print("Id "+d.Id);
            print("Line "+d.Line);
            print("To:");
            print("Id"+d.To.Id);
            print("Info"+d.To.Info);
            print("Line"+d.To.Line);
        }
    }
}