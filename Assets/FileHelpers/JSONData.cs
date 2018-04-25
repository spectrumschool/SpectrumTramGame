using UnityEngine;
using System.Collections;

public class JSONData<T>
{
    public static T CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<T>(jsonString);
    }

    public void Load(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, this);
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }
}