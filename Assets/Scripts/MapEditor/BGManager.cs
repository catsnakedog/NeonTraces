using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    DataManager Data;

    GameObject player;
    GameObject _camera;
    public GameObject[] BGObject = new GameObject[2];
    public GameObject BG01Object;

    SpriteRenderer spriteRenderer;

    public Action actionM;

    [SerializeField] private List<GameObject> BGs = new List<GameObject>();

    int stage;

    float camsize = 0;
    float Xmax = 0;
    float Xmin = 10000;
    float Ymax = 0;
    float Ymin = 10000;

    float[] BGMoveX = new float[2];
    float[] BGMoveY = new float[2];

    float[] DefaultX = new float[2];
    float[] DefaultY = new float[2];

    float[] xCorrection = new float[2];
    float[] yCorrection = new float[2];

    public float[] xPlus = new float[2];
    public float[] yPlus = new float[2];

    [SerializeField] private float pixelPerUnit = 7;
    void Start()
    {
        Data = DataManager.data;
    }

    void FixedUpdate()
    {
        actionM?.Invoke();
    }

    public void StartSetting()
    {
        Data = DataManager.data;
        _camera = GameObject.Find("MainCamera");
        camsize = Data.saveData.gameData.camsize;
    }

    public void BGSetting()
    {
        stage = Data.saveData.gameData.stage;
        player = Data.saveData.gameData.player;
        for (int i = 0; i < Data.saveData.mapData[stage].moveDots.Count; i++)
        {
            if (Data.saveData.mapData[stage].moveDots[i].v3.x >= Xmax)
            {
                Xmax = Data.saveData.mapData[stage].moveDots[i].v3.x;
            }
            if (Data.saveData.mapData[stage].moveDots[i].v3.x <= Xmin)
            {
                Xmin = Data.saveData.mapData[stage].moveDots[i].v3.x;
            }
            if (Data.saveData.mapData[stage].moveDots[i].v3.y >= Ymax)
            {
                Ymax = Data.saveData.mapData[stage].moveDots[i].v3.y;
            }
            if (Data.saveData.mapData[stage].moveDots[i].v3.y <= Ymin)
            {
                Ymin = Data.saveData.mapData[stage].moveDots[i].v3.y;
            }
        }

        switch(Data.saveData.gameData.stage)
        {
            case 0:
                BGObject = new GameObject[2];
                VariableSetting(1);
                BGObject[0] = Instantiate(BG01Object, new Vector3(0, 0, 0f), Quaternion.identity, GameObject.Find("MainCamera").transform);
                BGObject[1] = Instantiate(BGs[1], new Vector3(0, 0, 0f), Quaternion.identity, GameObject.Find("MainCamera").transform);
                BGObject[1].transform.position = new Vector3(GameObject.Find("MainCamera").transform.position.x, GameObject.Find("MainCamera").transform.position.y, 0);
                BGObject[0].transform.position = new Vector3(GameObject.Find("MainCamera").transform.position.x, GameObject.Find("MainCamera").transform.position.y, 0);
                break;
            case 1:
                goto case 0;
            case 2:
                BGObject = new GameObject[2];

                VariableSetting(0);
                VariableSetting(1);
                BGObject[0] = Instantiate(BGs[0], new Vector3(DefaultX[0], DefaultY[0], 0f), Quaternion.identity, GameObject.Find("MainCamera").transform);
                BGObject[1] = Instantiate(BGs[1], new Vector3(0, 0, 0f), Quaternion.identity, GameObject.Find("MainCamera").transform);
                BGObject[1].transform.position = new Vector3(GameObject.Find("MainCamera").transform.position.x, GameObject.Find("MainCamera").transform.position.y, 0);

                actionM += BGMoveSetting;
                break;
            case 3:
                goto case 2;
            default:
                goto case 1;
        }
    }

    void VariableSetting(int num)
    {
        spriteRenderer = BGs[num].GetComponent<SpriteRenderer>();
        BGMoveX[num] = ((spriteRenderer.sprite.rect.size.x + xPlus[num]) / 7f - camsize * 4f) / (Xmax - Xmin);
        if ((Ymax - Ymin) == 0)
        {
            BGMoveY[num] = 0;
        }
        else
        {
            BGMoveY[num] = ((spriteRenderer.sprite.rect.size.y + yPlus[num]) / 7f - camsize * 2f) / (Ymax - Ymin);
        }

        xCorrection[num] = ((spriteRenderer.sprite.rect.size.x + xPlus[num]) / 7f - camsize * 4f) / 2;
        yCorrection[num] = ((spriteRenderer.sprite.rect.size.y + yPlus[num]) / 7f - camsize * 2f) / 2;
        DefaultX[num] = _camera.transform.position.x + xCorrection[num];
        DefaultY[num] = _camera.transform.position.y + yCorrection[num];
    }

    void BGMoveSetting()
    {
        BGMove(0);
    }

    void BGMove(int num)
    {
        Vector3 cameraV3 = new Vector3(_camera.transform.position.x + xCorrection[num] - ((player.transform.position.x - Xmin) * BGMoveX[num]), _camera.transform.position.y + yCorrection[num] - ((player.transform.position.y - Ymin) * BGMoveY[num]), 0f);
        BGObject[num].transform.position = cameraV3;
    }
}
