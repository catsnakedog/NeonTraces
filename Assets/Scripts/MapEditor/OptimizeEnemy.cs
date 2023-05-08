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

    public void OptimizeStart()
    {
        player = Data.saveData.gameData.player;
        camera = player.transform.GetChild(1).gameObject;
        optimizeAction += SetActiveEnemy;
    }

    void SetActiveEnemy() // x축 기준으로 화면 * 1.1안에 들어온 오브젝트를 활성화 시킨다
    {
        for (int i = 0; i < Data.saveData.gameData.enemyInfo.Count; i++)
        {
            if ((Data.saveData.gameData.enemyInfo[i].x > (camera.transform.position.x - 11f)) && (Data.saveData.gameData.enemyInfo[i].x < (camera.transform.position.x + 11f)))
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(true);
            }
            else
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(false);
            }
        }
    }

    public void StopOptimize()
    {
        optimizeAction -= SetActiveEnemy;
    }
}
