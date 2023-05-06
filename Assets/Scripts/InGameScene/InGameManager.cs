using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    MapMake mapMake;
    DataManager Data;
    PlayerMove playerMove;
    int stage;
    // Start is called before the first frame update
    void Start()
    {
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerMove = gameObject.GetComponent<PlayerMove>();
        Data = DataManager.data;
        Data.saveData.gameData.stage = 0; // 스테이지 임시로 지정
        stage = Data.saveData.gameData.stage;
        Invoke("SettingGame", 1f);
    }

    void SettingGame()
    {
        mapMake.EnemySetting(stage);
        mapMake.EventDotSetting(stage);
        playerMove.MoveStart();
    }
}
