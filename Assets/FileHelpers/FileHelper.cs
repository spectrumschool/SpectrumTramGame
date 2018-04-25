using UnityEngine;
using System;
using System.IO;
using System.Text;

public static class FileHelper
{

    public static string RelativeFolder(string folder1, string folder2 = null, string folder3 = null, string folder4 = null, string folder5 = null)
    {
        StringBuilder sb = new StringBuilder(folder1);

        if (!string.IsNullOrEmpty(folder2)) sb.Append(Path.DirectorySeparatorChar + folder2);
        else return sb.ToString();

        if (!string.IsNullOrEmpty(folder3)) sb.Append(Path.DirectorySeparatorChar + folder3);
        else return sb.ToString();

        if (!string.IsNullOrEmpty(folder4)) sb.Append(Path.DirectorySeparatorChar + folder4);
        else return sb.ToString();

        if (!string.IsNullOrEmpty(folder5)) sb.Append(Path.DirectorySeparatorChar + folder5);
        else return sb.ToString();

        return sb.ToString();
    }

    public static string FilePath(string path, string fileName, string extension)
    {
        return path + Path.DirectorySeparatorChar + fileName + "." + extension;
    }

    public static string FolderPath(Environment.SpecialFolder specialFolder, string relativePath)
    {
        return Environment.GetFolderPath(specialFolder) + Path.DirectorySeparatorChar + relativePath;
    }

    public static string FolderPath(string path, string relativePath)
    {
        return path + Path.DirectorySeparatorChar + relativePath;
    }

    public static string PathToApplication()
    {
        return Path.GetFullPath(".");
    }

    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    public static string ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("> file not found: " + path);
            return null;
        }

        try
        {
            string result = File.ReadAllText(path);
            Debug.Log("> read file: " + path);
            return result;
        }
        catch (Exception e)
        {
            Debug.LogWarning("> read file failed: " + path + "\n" + e.ToString());
        }

        return null;
    }

    public static bool WriteFile(string path, string text)
    {
        try
        {
            File.WriteAllText(path, text);
            Debug.Log("> wrote file: " + path);
        }
        catch (Exception e)
        {
            Debug.LogWarning("> write file failed: " + path + "\n" + e.ToString());
            return false;
        }

        return true;
    }

    public static bool CreateFolder(string path)
    {
        try
        {
            if (Directory.Exists(path))
            {
                Debug.Log("> folder already exists: " + path);
                return true;
            }
            
            Directory.CreateDirectory(path);
            Debug.Log("> folder created: " + path);
        }
        catch (Exception e)
        {
            Debug.Log("> folder creation failed: "+ path +"\n"+ e.ToString());
            return false;
        }

        return true;
    }
}