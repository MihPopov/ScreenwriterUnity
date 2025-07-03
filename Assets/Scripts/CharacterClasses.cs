using System;
using System.Collections.Generic;

[Serializable]
public class CharacterClass
{
    public string Id;
    public string Name;
    public List<DataClass> Data;
}

[Serializable]
public class DataClass
{
    public string Id;
    public string Line;
    public ToClass To;
}

[Serializable]
public class ToClass
{
    public string Id;
    public string Info;
    public string Line;
}