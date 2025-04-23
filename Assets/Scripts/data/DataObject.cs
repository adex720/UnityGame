using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject
{

    public static readonly DataObject LEVELS_COMPLETED = new DataObject(0, "levels_completed", "0");


    public readonly int id;
    public readonly string name;
    public readonly string defaultValue;

    public DataObject(int id, string name, string defaultValue)
    {
        this.id = id;
        this.name = name;
        this.defaultValue = defaultValue;

        DataManager.AddDataObject(this);
    }

    public static void Init() { }

}