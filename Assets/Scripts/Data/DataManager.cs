using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    JsonManager jsonManager;
    public SaveDataClass saveData;
    public static DataManager data;

    void Awake()
    {
        if (data == null)
        {
            data = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        jsonManager = new JsonManager();
        saveData = new SaveDataClass();

        Load();
    }
    public void Save()
    {
        jsonManager.SaveJson(saveData);
    }

    public void Load()
    {
        saveData = jsonManager.LoadSaveData();
    }
}

