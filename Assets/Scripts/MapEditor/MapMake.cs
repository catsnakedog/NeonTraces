using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapMake : MonoBehaviour
{
    [SerializeField] private GameObject moveDot;
    [SerializeField] private List<GameObject> enemy;
    [SerializeField] private GameObject eventDot;
    [SerializeField] private GameObject bezierDot;
    [SerializeField] private GameObject blood;
    [SerializeField] private GameObject bloodBoom;

    public bool isMapEditor;

    List<GameObject> moveDotList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> eventDotList = new List<GameObject>();
    List<GameObject> bezierDotList = new List<GameObject>();

    GameObject map;
    GameObject moveDots;
    GameObject enemys;
    GameObject eventDots;
    GameObject bezierDots;
    [SerializeField] private List<GameObject> mapBGs = new List<GameObject>();
    public List<LineRenderer> lrs = new List<LineRenderer>();
    public int lrCount = 0;
    public int bezierCount = 0;
    EnemyInfo enemyInfo;
    [SerializeField] private InputField stageInput;
    [SerializeField] private Material lrDefault;

    DataManager Data;

    BGManager bgManager;
    [SerializeField] private int stage;

    bool isMapMake = false;
    bool isMapClear = true;

    void Start()
    {
        isMapEditor = true;    
    }

    public void StartSetting()
    {
        Data = DataManager.data;
        Data.saveData.gameData.blood = blood;
        Data.saveData.gameData.bloodBoom = bloodBoom;
        stage = Data.saveData.gameData.stage;
        map = GameObject.Find("Map");
        bgManager = GameObject.Find("InGameManager").GetComponent<BGManager>();
        Data.saveData.gameData.map = map;
        MapGameObjectMake();
        isMapEditor = true;
    }

    public void MapGameObjectMake()
    {
        GameObject moveDots = new GameObject();
        moveDots.transform.SetParent(map.transform);
        moveDots.name = "moveDots";
        GameObject enemys = new GameObject();
        enemys.transform.SetParent(map.transform);
        enemys.name = "enemys";
        GameObject eventDots = new GameObject();
        eventDots.transform.SetParent(map.transform);
        eventDots.name = "eventDots";
        GameObject bezierDots = new GameObject();
        bezierDots.name = "bezierDots";

        this.moveDots = moveDots;
        this.enemys = enemys;
        this.eventDots = eventDots;
        this.bezierDots = bezierDots;
    }

    public void MapSetting() // 맵세팅
    {
        if(isMapClear)
        {
            lrCount = 0;
            bezierCount = 0;
            stage = Data.saveData.gameData.stage;
            LineRendererSetting(0);
            lrCount++;
            if(isMapEditor)
            {
                MoveDotSetting(stage);
            }    
            EnemySetting(stage);
            EventDotSetting(stage);
            GameObject mapBG = Instantiate(mapBGs[stage], new Vector3(14f, 5f, 0f), Quaternion.identity);
            mapBG.transform.SetParent(map.transform);
            bgManager.BGSetting();
            isMapMake = true;
            isMapClear = false;
        }
        else
        {
            Debug.Log("실수로 두번 클릭한 당신.. 이번만 살려드리는겁니다..");
        }
    }

    public void MapDelete() // 맵삭제 (맵 에디터 용)
    {
        if(isMapMake)
        {
            bgManager.actionM = null;
            Destroy(map.transform.GetChild(3).gameObject);
            Destroy(bgManager.BGObject);
            DestroyAll();
        }
        else
        {
            Debug.Log("실수로 두번 클릭한 당신.. 이번만 살려드리는겁니다..");
        }
    }


    void DestroyAll() // 맵요소 전부 파괴 (맵 에디터 용)
    {
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name != transform.name)
            {
                Destroy(child.gameObject);
            }
        }
        Destroy(moveDots.gameObject);
        Destroy(enemys.gameObject);
        Destroy(eventDots.gameObject);
        Destroy(bezierDots.gameObject);
        lrs.Clear();
        moveDotList.Clear();
        enemyList.Clear();
        eventDotList.Clear();
        bezierDotList.Clear();

        MapGameObjectMake();
        isMapMake = false;
        isMapClear = true;
    }

    // 맵 세팅 관련 함수들 / 맵 데이터를 읽어와서 오브젝트를 배치한다
    #region MapSetting
    void MoveDotSetting(int stage) // 시각적으로 어느 위치로 움직이는지 확인하기 위해 점들을 세팅해준다 (맵 에디터 용)
    {
        lrs[0].SetPosition(0, Data.saveData.mapData[stage].moveDots[0].v3);
        lrs[0].positionCount = Data.saveData.mapData[stage].moveDots.Count;
        lrs[0].enabled = true;
        for (int i = 0; i < Data.saveData.mapData[stage].moveDots.Count; i++)
        {
            lrs[0].SetPosition(i, Data.saveData.mapData[stage].moveDots[i].v3);
            moveDotList.Add(Instantiate(moveDot, Data.saveData.mapData[stage].moveDots[i].v3, Quaternion.identity));
            moveDotList[i].transform.SetParent(moveDots.transform, true);
            moveDotList[i].name = "moveDot" + i.ToString();
            moveDotList[i].GetComponent<MoveDotInfo>().speed = Data.saveData.mapData[stage].moveDots[i].speed;
        }
    }

    public void EnemySetting(int stage) // 적들을 세팅해준다
    {
        Data.saveData.gameData.enemyPoint.Clear();
        Data.saveData.gameData.enemyInfo.Clear();
        for (int i = 0; i < Data.saveData.mapData[stage].enemys.Count; i++)
        {
            enemyList.Add(Instantiate(enemy[Data.saveData.mapData[stage].enemys[i].type], Data.saveData.mapData[stage].enemys[i].v3, Quaternion.identity));
            enemyList[i].transform.GetChild(0).position = Data.saveData.mapData[stage].enemys[i].moveV3;
            EnemyInfo temp = new EnemyInfo(enemyList[i], Data.saveData.mapData[stage].enemys[i].v3.x, i);
            Data.saveData.gameData.enemyInfo.Add(temp);
            enemyList[i].GetComponent<EnemySetting>().index = i;
            enemyList[i].GetComponent<EnemySetting>().EnemyPosition();
            enemyList[i].transform.SetParent(enemys.transform, true);
            enemyList[i].name = "enemyDot" + i.ToString();
            Data.saveData.gameData.enemyPoint.Add(enemyList[i].transform.GetChild(0).gameObject);
        }
    }

    public void EventDotSetting(int stage) // 이벤트포인트를 세팅해준다
    {
        for (int i = 0; i < Data.saveData.mapData[stage].eventDots.Count; i++)
        {
            eventDotList.Add(Instantiate(eventDot, Data.saveData.mapData[stage].eventDots[i].v3, Quaternion.identity));
            eventDotList[i].transform.SetParent(eventDots.transform, true);
            eventDotList[i].GetComponent<EventDotSetting>().type = Data.saveData.mapData[stage].eventDots[i].type;
            eventDotList[i].GetComponent<EventDotSetting>().eventTypeInfo = Data.saveData.mapData[stage].eventDots[i].eventTypeInfo;
            eventDotList[i].name = "eventDot" + i.ToString();
            if (Data.saveData.mapData[stage].eventDots[i].type == 0)
            { 
                if(isMapEditor)
                {
                    lrBezierSetting(Data.saveData.mapData[stage].eventDots[i].v3, Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.pointDot1, Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.pointDot2, Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.nextMoveDot].v3);
                }
            }
            if (!isMapEditor)
                eventDotList[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    void LineRendererSetting(int lrNumber) // 시각적으로 어느 위치로 움직이는지 확인하기 위해 점들 사이를 직선으로 이어준다 (맵 에디터 용)
    {
        GameObject lineRenderer = new GameObject("LineRenderer"+lrNumber);
        lineRenderer.transform.SetParent(gameObject.transform, true);
        gameObject.transform.GetChild(lrNumber).gameObject.AddComponent<LineRenderer>();
        lrs.Add(gameObject.transform.GetChild(lrNumber).gameObject.GetComponent<LineRenderer>());
        lrs[lrNumber].enabled = false;
        lrs[lrNumber].startColor = moveDot.GetComponent<SpriteRenderer>().color;
        lrs[lrNumber].endColor = moveDot.GetComponent<SpriteRenderer>().color;
        lrs[lrNumber].material = lrDefault;
        lrs[lrNumber].startWidth = 0.1f;
        lrs[lrNumber].endWidth = 0.1f;
    }

    public void lrBezierSetting(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) // 곡선 이동 경로를 표현함 (맵 에디터 용)
    {
        bezierDotList.Add(Instantiate(bezierDot, v2, Quaternion.identity));
        bezierDotList.Add(Instantiate(bezierDot, v3, Quaternion.identity));
        bezierDotList[bezierCount*2].transform.SetParent(bezierDots.transform, true);
        bezierDotList[bezierCount*2 + 1].transform.SetParent(bezierDots.transform, true);
        bezierCount++;
        LineRendererSetting(lrCount);
        lrCount++;
        int count = 50;
        lrs[lrCount - 1].positionCount = count+1;
        lrs[lrCount - 1].enabled = true;
        float tempF = 0;
        for (int i=0; i<count+1; i++)
        {
            float time = tempF / count;
            Vector3 A = Vector3.Lerp(v1, v2, time);
            Vector3 B = Vector3.Lerp(v2, v3, time);
            Vector3 C = Vector3.Lerp(v3, v4, time);
            Vector3 D = Vector3.Lerp(A, B, time);
            Vector3 E = Vector3.Lerp(B, C, time);
            Vector3 F = Vector3.Lerp(D, E, time);
            lrs[lrCount - 1].SetPosition(i, F);
            tempF += 1;
        }
    }
    #endregion
    public void SetStage() // 스테이지를 입력받아 변경시킴 (맵 에디터 용)
    {
        Data.saveData.gameData.stage = int.Parse(stageInput.text);
    }

    public void SaveObject() // 오브젝트들을 읽어와서 전부 저장한다 (맵 에디터 용)
    {
        Data.saveData.mapData[stage].moveDots.Clear();
        for(int i=0; i< moveDots.transform.childCount; i++)
        {
            GameObject saveDot = moveDots.transform.GetChild(i).gameObject;
            MoveDot temp = new MoveDot(saveDot.transform.position, saveDot.GetComponent<MoveDotInfo>().speed);
            Data.saveData.mapData[stage].moveDots.Add(temp);
        }
        Data.saveData.mapData[stage].enemys.Clear();
        for (int i = 0; i < enemys.transform.childCount; i++)
        {
            GameObject saveEnemy = enemys.transform.GetChild(i).gameObject;
            Enemy temp;
            if (saveEnemy.GetComponent<EnemySetting>().defaultV3 != saveEnemy.transform.position)
            {
                temp = new Enemy(saveEnemy.transform.position, saveEnemy.GetComponent<EnemySetting>().type, saveEnemy.transform.GetChild(0).position);
            }
            else
            {
                temp = new Enemy(saveEnemy.GetComponent<EnemySetting>().defaultV3, saveEnemy.GetComponent<EnemySetting>().type, saveEnemy.transform.GetChild(0).position);
            }
            Data.saveData.mapData[stage].enemys.Add(temp);
        }
        Data.saveData.mapData[stage].eventDots.Clear();
        for (int i = 0; i < eventDots.transform.childCount; i++)
        {
            GameObject saveEventDot = eventDots.transform.GetChild(i).gameObject;
            EventDot temp = new EventDot(saveEventDot.transform.position, saveEventDot.GetComponent<EventDotSetting>().type, saveEventDot.GetComponent<EventDotSetting>().eventTypeInfo);
            Data.saveData.mapData[stage].eventDots.Add(temp);
        }
        Data.Save();
        Data.Load();
        MapDelete();
        Invoke("MapSetting", 2f);
    }
}
