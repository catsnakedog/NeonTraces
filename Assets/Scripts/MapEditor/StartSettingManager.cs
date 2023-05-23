using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSettingManager : MonoBehaviour
{
    DataManager Data;
    AfterImage afterImage;
    CutSceneManager cutSceneManager;
    MapMake mapMake;
    OptimizeEnemy optimizeEnemy;
    PlayerAction playerAction;
    PlayerAnimaiton playerAnimaiton;
    PlayerMove playerMove;

    GameObject player;
    void Start()
    {
        Data = DataManager.data;
        playerMove = gameObject.GetComponent<PlayerMove>();
        playerAction = gameObject.GetComponent<PlayerAction>();
        optimizeEnemy = gameObject.GetComponent<OptimizeEnemy>();
        cutSceneManager = gameObject.GetComponent<CutSceneManager>();
        afterImage = gameObject.GetComponent<AfterImage>();
        playerAnimaiton = gameObject.GetComponent<PlayerAnimaiton>();
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();

        Data.saveData.gameData.camsize = 10;
        Data.saveData.gameData.stage = 0; // 임시로 추가

        player = GameObject.Find("Player");
        Data.saveData.gameData.player = player;

        mapMake.StartSetting();
        playerMove.StartSetting();
        playerAnimaiton.AniSet();
        playerAction.StartSetting();
    }
}