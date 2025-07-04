using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Characters
{
    public List<CharacterItem> chars;
}

[Serializable]
public class CharacterItem
{
    [field: SerializeField]
    public int id;
    [field: SerializeField]
    public string name;
    [field: SerializeField]
    public List<DataClass> data;
}

[Serializable]
public class DataClass
{
    [field: SerializeField]
    public int id;
    [field: SerializeField]
    public string line;
    [field: SerializeField]
    public List<ToClass> to;
}

[Serializable]
public class ToClass
{
    [field: SerializeField]
    public string id;
    [field: SerializeField]
    public string info;
    [field: SerializeField]
    public string line;
}