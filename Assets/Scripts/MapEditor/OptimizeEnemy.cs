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

    public void OptimizeStart() // 적 최적화 시작
    {
        player = Data.saveData.gameData.player;
        camera = GameObject.Find("MainCamera");
        optimizeAction += SetActiveEnemy;
    }

    void SetActiveEnemy() // x축 기준으로 화면 * 3안에 들어온 오브젝트를 활성화 시킨다
    {
        for (int i = 0; i < Data.saveData.gameData.enemyInfo.Count; i++)
        {
            if ((Data.saveData.gameData.enemyInfo[i].x > (camera.transform.position.x - Data.saveData.gameData.camsize*5f)) && (Data.saveData.gameData.enemyInfo[i].x < (camera.transform.position.x + Data.saveData.gameData.camsize * 5f)))
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(true);
            }
            else
            {
                Data.saveData.gameData.enemyInfo[i].enemy.SetActive(false);
            }
        }
    }

    public void StopOptimize() // 최적화 중지
    {
        optimizeAction -= SetActiveEnemy;
    }
}
