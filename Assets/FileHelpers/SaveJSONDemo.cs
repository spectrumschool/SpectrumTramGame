using UnityEngine;
using System;

[Serializable]
public class SaveMeToo : JSONData<SaveMeToo>
{
    public float height;
}

[Serializable]
public class SaveMe : JSONData<SaveMe>
{
    public Vector3 position;
    public string firstName;
    public int age;
    public SaveMeToo[] saveMeToo;
}

public class SaveJSONDemo : MonoBehaviour
{
    public Environment.SpecialFolder specialFolder = Environment.SpecialFolder.MyDocuments;
    public string folderName = "SaveJSONDemo";
    public SaveMe saveMe;

    void Start ()
    {
        string folderPath = FileHelper.FolderPath(specialFolder, folderName);
        FileHelper.CreateFolder(folderPath);

        string filePath = FileHelper.FilePath(folderPath,"bestandsnaam","json");
        FileHelper.WriteFile(filePath, saveMe.SaveToString());
	}
}
