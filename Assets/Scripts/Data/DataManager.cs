using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    JsonManager jsonManager; // json���� ���� �о���ų� �����ϴ� JsonManager
    public SaveDataClass saveData; // �����͸� �����ϴ� ������ SaveDataClass
    public static DataManager data;

    void Awake()
    {
        if (data == null) // DataManager�� ���ϼ� ����
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
    public void Save() // saveData�� ��ϵ� �����͵��� json�� �����Ѵ�
    {
        jsonManager.SaveJson(saveData);
    }

    public void Load() // json�� ��ϵ��ִ� �����͵��� saveData�� �����´�
    {
        saveData = jsonManager.LoadSaveData();
    }
}

