using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    MapMake mapMake;
    DataManager Data;
    PlayerMove playerMove;
    OptimizeEnemy optimizeEnemy;
    int stage;
    // Start is called before the first frame update
    void Start()
    {
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerMove = gameObject.GetComponent<PlayerMove>();
        optimizeEnemy = gameObject.GetComponent<OptimizeEnemy>();
        Data = DataManager.data;
        stage = Data.saveData.gameData.stage;
        Invoke("SettingGame", 1f);
    }

    void SettingGame() // ���� ���� �Լ�, �ʻ��� �÷��̾� ������ ����� �Ѵ� / �Ƹ��� �ʻ����� �������� �Ҹ��ؼ� �����ҰŰ��� (���� �ʿ�)
    {
        mapMake.isMapEditor = false;
        mapMake.MapSetting();
        optimizeEnemy.OptimizeStart();
        playerMove.Invoke("MoveStart", 1f);
    }
}
