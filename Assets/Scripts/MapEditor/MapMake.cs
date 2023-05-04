using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

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
    DataManager Data;
    [SerializeField] private int stage;
    void Start()
    {
        Data = DataManager.data;
        map = GameObject.Find("Map");
        moveDots = map.transform.GetChild(1).gameObject;
        enemys = map.transform.GetChild(0).gameObject;
        eventDots = map.transform.GetChild(2).gameObject;
    }

    public void MapSetting()
    {
        MoveDotSetting(stage);
        EnemySetting(stage);
        EventDotSetting(stage);
    }

    public void MapDelete()
    {
        DestroyAll(moveDotList);
        DestroyAll(enemyList);
        DestroyAll(eventDotList);
    }


    void DestroyAll(List<GameObject> target)
    {
        for (int i = 0; i < target.Count; i++)
        {
            Destroy(target[i]);
        }
        target.Clear();
    }

    void MoveDotSetting(int stage)
    {
        for (int i = 0; i < Data.saveData.mapData[stage].moveDots.Count; i++)
        {
            moveDotList.Add(Instantiate(moveDot, Data.saveData.mapData[stage].moveDots[i].v3, Quaternion.identity));
            moveDotList[i].transform.SetParent(moveDots.transform, true);
        }
    }

    void EnemySetting(int stage)
    {
        for (int i = 0; i < Data.saveData.mapData[stage].enemys.Count; i++)
        {
            enemyList.Add(Instantiate(enemy[Data.saveData.mapData[stage].enemys[i].type], Data.saveData.mapData[0].enemys[i].v3, Quaternion.identity));
            enemyList[i].transform.SetParent(enemys.transform, true);
        }
    }

    void EventDotSetting(int stage)
    {
        for (int i = 0; i < Data.saveData.mapData[stage].eventDots.Count; i++)
        {
            eventDotList.Add(Instantiate(eventDot[Data.saveData.mapData[stage].eventDots[i].type], Data.saveData.mapData[0].eventDots[i].v3, Quaternion.identity));
            eventDotList[i].transform.SetParent(eventDots.transform, true);
        }
    }
}
