using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager
{

    private static List<DataObject> dataObjects = new List<DataObject>();
    private List<string> data;

    private readonly string filePath;

    public DataManager(string path)
    {
        DataObject.Init();

        filePath = path;
        data = new List<string>();
        Load();
    }

    public static void AddDataObject(DataObject obj)
    {
        dataObjects.Add(obj);
    }

    public void Save()
    {
        File.WriteAllText(filePath, GetAsString());
    }

    public string GetAsString()
    {
        string s = "";
        foreach (string v in data)
        {
            s += v + "\n";
        }

        return s;
    }

    public void Load()
    {
        data.Clear();

        string content = GetOrCreateFile();
        string[] values = content.Split("\n");

        for (int i = 0; i < dataObjects.Count; i++)
        {
            if (i < values.Length && values[i].Length > 0) data.Add(values[i]);
            else data.Add(dataObjects[i].defaultValue);
        }
    }

    string GetOrCreateFile()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Creating new savedata file at: " + filePath);
            File.Create(filePath).Dispose();
        }

        return System.IO.File.ReadAllText(filePath);
    }

    public void SaveString(int id, string value)
    {
        data[id] = value;
        Save();
    }

    public void SaveString(DataObject dataObject, string value)
    {
        SaveString(dataObject.id, value);
    }

    public void SaveInt(int id, int value)
    {
        SaveString(id, "" + value);
    }

    public void SaveInt(DataObject dataObject, int value)
    {
        SaveInt(dataObject.id, value);
    }

    public void SaveFloat(int id, float value)
    {
        SaveString(id, "" + value);
    }

    public void SaveFloat(DataObject dataObject, float value)
    {
        SaveFloat(dataObject.id, value);
    }


    public string GetString(int id)
    {
        // Debug.Log("id: " + id + ", arvo: " + data[id]);
        return data[id];
    }

    public string GetString(DataObject dataObject)
    {
        return GetString(dataObject.id);
    }

    public int GetInt(int id)
    {
        string asString = GetString(id);
         // Debug.Log("string: " + asString + ", pituus: " + asString.Length);
        if (asString.Length == 0) return 0;

        int result;
        bool isValid = int.TryParse(asString, out result);
         // Debug.Log("isValid: " + isValid + ", result: " + result);
        if (!isValid)
        {
            return -1;
        }
        return result;
    }

    public int GetInt(DataObject dataObject)
    {
        return GetInt(dataObject.id);
    }

    public float GetFloat(int id)
    {
        string asString = GetString(id);
        if (asString.Length == 0) return 0F;

        float result;
        bool isValid = float.TryParse(asString, out result);
        if (!isValid)
        {
            return -1;
        }
        return result;
    }

    public float GetFloat(DataObject dataObject)
    {
        return GetFloat(dataObject.id);
    }


    public void IncreaseInt(int id, int amount)
    {
        SaveInt(id, GetInt(id) + amount);
    }

    public void IncreaseInt(DataObject dataObject, int amount)
    {
        IncreaseInt(dataObject.id, amount);
    }

    public void IncreaseFloat(int id, float amount)
    {
        SaveFloat(id, GetFloat(id) + amount);
    }

    public void IncreaseFloat(DataObject dataObject, float amount)
    {
        IncreaseFloat(dataObject.id, amount);
    }

}