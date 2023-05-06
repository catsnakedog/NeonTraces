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

    void SettingGame() // 게임 시작 함수, 맵생성 플레이어 움직임 등등을 한다 / 아마도 맵생성과 움직임은 불리해서 실행할거같다 (수정 필요)
    {
        mapMake.EnemySetting(stage);
        mapMake.EventDotSetting(stage);
        playerMove.MoveStart();
    }
}
