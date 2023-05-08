using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MapMake : MonoBehaviour
{
    [SerializeField] private GameObject moveDot;
    [SerializeField] private List<GameObject> enemy;
    [SerializeField] private List<GameObject> eventDot;

    List<GameObject> moveDotList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> eventDotList = new List<GameObject>();

    GameObject map;
    GameObject moveDots;
    GameObject enemys;
    GameObject eventDots;
    LineRenderer lr;
    EnemyInfo enemyInfo;
    [SerializeField] private InputField stageInput;
    [SerializeField] private Material lrDefault;

    DataManager Data;
    [SerializeField] private int stage;

    void Start()
    {
        Data = DataManager.data;
        stage = Data.saveData.gameData.stage;
        map = GameObject.Find("Map");
        moveDots = map.transform.GetChild(1).gameObject;
        enemys = map.transform.GetChild(0).gameObject;
        eventDots = map.transform.GetChild(2).gameObject;
    }

    public void MapSetting() // 맵세팅 (맵 에디터 용)
    {
        stage = Data.saveData.gameData.stage;
        RineRendererSetting();
        MoveDotSetting(stage);
        EnemySetting(stage);
        EventDotSetting(stage);
    }

    public void MapDelete() // 맵삭제 (맵 에디터 용)
    {
        lr.enabled = false;
        Destroy(lr);
        DestroyAll(moveDotList);
        DestroyAll(enemyList);
        DestroyAll(eventDotList);
    }


    void DestroyAll(List<GameObject> target) // 리스트 안에 있는 모든 게임오브젝트를 파괴한다
    {
        for (int i = 0; i < target.Count; i++)
        {
            Destroy(target[i]);
        }
        target.Clear();
    }

    void MoveDotSetting(int stage) // 시각적으로 어느 위치로 움직이는지 확인하기 위해 점들을 세팅해준다 (맵 에디터 용)
    {
        lr.SetPosition(0, Data.saveData.mapData[stage].moveDots[0].v3);
        lr.positionCount = Data.saveData.mapData[stage].moveDots.Count;
        lr.enabled = true;
        for (int i = 0; i < Data.saveData.mapData[stage].moveDots.Count; i++)
        {
            lr.SetPosition(i, Data.saveData.mapData[stage].moveDots[i].v3);
            moveDotList.Add(Instantiate(moveDot, Data.saveData.mapData[stage].moveDots[i].v3, Quaternion.identity));
            moveDotList[i].transform.SetParent(moveDots.transform, true);
        }
    }

    public void EnemySetting(int stage) // 적들을 세팅해준다
    {
        Data.saveData.gameData.enemyInfo.Clear();
        for (int i = 0; i < Data.saveData.mapData[stage].enemys.Count; i++)
        {
            enemyList.Add(Instantiate(enemy[Data.saveData.mapData[stage].enemys[i].type], Data.saveData.mapData[stage].enemys[i].v3, Quaternion.identity));
            EnemyInfo temp = new EnemyInfo(enemyList[i], Data.saveData.mapData[stage].enemys[i].v3.x, i);
            Data.saveData.gameData.enemyInfo.Add(temp);
            enemyList[i].transform.SetParent(enemys.transform, true);
        }
    }

    public void EventDotSetting(int stage) // 이벤트포인트를 세팅해준다
    {
        for (int i = 0; i < Data.saveData.mapData[stage].eventDots.Count; i++)
        {
            eventDotList.Add(Instantiate(eventDot[Data.saveData.mapData[stage].eventDots[i].type], Data.saveData.mapData[stage].eventDots[i].v3, Quaternion.identity));
            eventDotList[i].transform.SetParent(eventDots.transform, true);
        }
    }

    void RineRendererSetting() // 시각적으로 어느 위치로 움직이는지 확인하기 위해 점들 사이를 직선으로 이어준다 (맵 에디터 용)
    {
        gameObject.transform.GetChild(0).gameObject.AddComponent<LineRenderer>();
        lr = gameObject.transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
        lr.enabled = false;
        lr.startColor = moveDot.GetComponent<SpriteRenderer>().color;
        lr.endColor = moveDot.GetComponent<SpriteRenderer>().color;
        lr.material = lrDefault;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
    }

    public void SetStage()
    {
        Data.saveData.gameData.stage = int.Parse(stageInput.text);
    }
}
