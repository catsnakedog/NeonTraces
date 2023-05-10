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
    [SerializeField] private GameObject eventDot;
    [SerializeField] private GameObject bezierDot;

    List<GameObject> moveDotList = new List<GameObject>();
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> eventDotList = new List<GameObject>();
    List<GameObject> bezierDotList = new List<GameObject>();

    GameObject map;
    GameObject moveDots;
    GameObject enemys;
    GameObject eventDots;
    GameObject bezierDots;
    public List<LineRenderer> lrs = new List<LineRenderer>();
    public int lrCount = 0;
    public int bezierCount = 0;
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
        moveDots = map.transform.GetChild(0).gameObject;
        enemys = map.transform.GetChild(1).gameObject;
        eventDots = map.transform.GetChild(2).gameObject;
        bezierDots = map.transform.GetChild(3).gameObject;
    }

    public void MapSetting() // �ʼ��� (�� ������ ��)
    {
        lrCount = 0;
        bezierCount = 0;
        stage = Data.saveData.gameData.stage;
        LineRendererSetting(0);
        lrCount++;
        MoveDotSetting(stage);
        EnemySetting(stage);
        EventDotSetting(stage);
    }

    public void MapDelete() // �ʻ��� (�� ������ ��)
    {
        lrs.Clear();
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child.name != transform.name)
            {
                Destroy(child.gameObject);
            }
        }
        DestroyAll(moveDotList);
        DestroyAll(enemyList);
        DestroyAll(eventDotList);
        DestroyAll(bezierDotList);
    }


    void DestroyAll(List<GameObject> target) // ����Ʈ �ȿ� �ִ� ��� ���ӿ�����Ʈ�� �ı��Ѵ�
    {
        for (int i = 0; i < target.Count; i++)
        {
            Destroy(target[i]);
        }
        target.Clear();
    }

    void MoveDotSetting(int stage) // �ð������� ��� ��ġ�� �����̴��� Ȯ���ϱ� ���� ������ �������ش� (�� ������ ��)
    {
        lrs[0].SetPosition(0, Data.saveData.mapData[stage].moveDots[0].v3);
        lrs[0].positionCount = Data.saveData.mapData[stage].moveDots.Count;
        lrs[0].enabled = true;
        for (int i = 0; i < Data.saveData.mapData[stage].moveDots.Count; i++)
        {
            lrs[0].SetPosition(i, Data.saveData.mapData[stage].moveDots[i].v3);
            moveDotList.Add(Instantiate(moveDot, Data.saveData.mapData[stage].moveDots[i].v3, Quaternion.identity));
            moveDotList[i].transform.SetParent(moveDots.transform, true);
        }
    }

    public void EnemySetting(int stage) // ������ �������ش�
    {
        Data.saveData.gameData.enemyInfo.Clear();
        for (int i = 0; i < Data.saveData.mapData[stage].enemys.Count; i++)
        {
            enemyList.Add(Instantiate(enemy[Data.saveData.mapData[stage].enemys[i].type], Data.saveData.mapData[stage].enemys[i].v3, Quaternion.identity));
            EnemyInfo temp = new EnemyInfo(enemyList[i], Data.saveData.mapData[stage].enemys[i].v3.x, i);
            Data.saveData.gameData.enemyInfo.Add(temp);
            enemyList[i].GetComponent<EnemySetting>().index = i;
            enemyList[i].transform.SetParent(enemys.transform, true);
        }
    }

    public void EventDotSetting(int stage) // �̺�Ʈ����Ʈ�� �������ش�
    {
        for (int i = 0; i < Data.saveData.mapData[stage].eventDots.Count; i++)
        {
            eventDotList.Add(Instantiate(eventDot, Data.saveData.mapData[stage].eventDots[i].v3, Quaternion.identity));
            eventDotList[i].transform.SetParent(eventDots.transform, true);
            eventDotList[i].GetComponent<EventDotSetting>().type = Data.saveData.mapData[stage].eventDots[i].type;
            eventDotList[i].GetComponent<EventDotSetting>().eventTypeInfo = Data.saveData.mapData[stage].eventDots[i].eventTypeInfo;
            if(Data.saveData.mapData[stage].eventDots[i].type == 0) // � �̵� ��θ� �����Ѵ� (�� ������ ��)
            { // ���� �ΰ������� �� ���������� �����ؼ� ���� ���Ѿ��� (���� ���� ����)
                lrBezierSetting(Data.saveData.mapData[stage].eventDots[i].v3, Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.pointDot1, Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.pointDot2, Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[Data.saveData.mapData[stage].eventDots[i].eventTypeInfo.type0.nextMoveDot].v3);
            }
        }
    }

    void LineRendererSetting(int lrNumber) // �ð������� ��� ��ġ�� �����̴��� Ȯ���ϱ� ���� ���� ���̸� �������� �̾��ش� (�� ������ ��)
    {
        GameObject lineRenderer = new GameObject("LineRenderer"+lrNumber);
        lineRenderer.transform.SetParent(gameObject.transform, true);
        gameObject.transform.GetChild(lrNumber).gameObject.AddComponent<LineRenderer>();
        lrs.Add(gameObject.transform.GetChild(lrNumber).gameObject.GetComponent<LineRenderer>());
        lrs[lrNumber].enabled = false;
        lrs[lrNumber].startColor = moveDot.GetComponent<SpriteRenderer>().color;
        lrs[lrNumber].endColor = moveDot.GetComponent<SpriteRenderer>().color;
        lrs[lrNumber].material = lrDefault;
        lrs[lrNumber].startWidth = 0.05f;
        lrs[lrNumber].endWidth = 0.05f;
    }

    public void lrBezierSetting(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) // � �̵� ��θ� ǥ���� (�� ������ ��)
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

    public void SetStage() // ���������� �Է¹޾� �����Ŵ (�� ������ ��)
    {
        Data.saveData.gameData.stage = int.Parse(stageInput.text);
    }
}
