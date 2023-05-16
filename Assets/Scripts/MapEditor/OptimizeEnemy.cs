using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Common;
using UnityEngine;

public class OptimizeEnemy : MonoBehaviour
{
    GameObject player;
    new GameObject camera;
    DataManager Data;

    Action optimizeAction;

    void Start()
    {
        Data = DataManager.data;
    }

    void FixedUpdate()
    {
        optimizeAction?.Invoke();
    }

    public void OptimizeStart() // �� ����ȭ ����
    {
        player = Data.saveData.gameData.player;
        camera = GameObject.Find("MainCamera");
        optimizeAction += SetActiveEnemy;
    }

    void SetActiveEnemy() // x�� �������� ȭ�� * 1.1�ȿ� ���� ������Ʈ�� Ȱ��ȭ ��Ų��
    {
        for (int i = 0; i < Data.saveData.gameData.enemyInfo.Count; i++)
        {
            if ((Data.saveData.gameData.enemyInfo[i].x > (camera.transform.position.x - Data.saveData.gameData.camsize*2.2f)) && (Data.saveData.gameData.enemyInfo[i].x < (camera.transform.position.x + Data.saveData.gameData.camsize * 2.2f)))
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(true);
            }
            else
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(false);
            }
        }
    }

    public void StopOptimize() // ����ȭ ����
    {
        optimizeAction -= SetActiveEnemy;
    }
}
