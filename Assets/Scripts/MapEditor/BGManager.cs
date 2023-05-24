using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BGManager : MonoBehaviour
{
    DataManager Data;

    GameObject player;
    GameObject camera;
    public GameObject BGObject;

    SpriteRenderer spriteRenderer;

    public Action actionM;

    [SerializeField] private List<GameObject> BGs = new List<GameObject>();

    int stage;

    float camsize = 0;
    float Xmax = 0;
    float Xmin = 10000;
    float Ymax = 0;
    float Ymin = 10000;

    float BGMoveX = 0;
    float BGMoveY = 0;

    float DefaultX = 0;
    float DefaultY = 0;

    public float xCorrection;
    public float yCorrection;

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
        camera = GameObject.Find("MainCamera");
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

        spriteRenderer = BGs[stage].GetComponent<SpriteRenderer>();
        BGMoveX = ((spriteRenderer.sprite.rect.size.x + xCorrection) / 7f - (camsize * 4f * pixelPerUnit) / 7f) / (Xmax - Xmin);
        BGMoveY = ((spriteRenderer.sprite.rect.size.y + yCorrection) / 7f - (camsize * 2f * pixelPerUnit) / 7f) / (Ymax - Ymin);
        DefaultX = camera.transform.position.x + ((spriteRenderer.sprite.rect.size.x + xCorrection) / 14f) - ((camsize * 2f * pixelPerUnit) / 7f);
        DefaultY = camera.transform.position.y + ((spriteRenderer.sprite.rect.size.y + yCorrection) / 14f) - ((camsize * pixelPerUnit) / 7f);
        BGObject = Instantiate(BGs[stage], new Vector3(DefaultX, DefaultY, 0f), Quaternion.identity);

        actionM += BGMoveSetting;
    }

    void BGMoveSetting()
    {
        Vector3 cameraV3 = new Vector3(camera.transform.position.x + ((spriteRenderer.sprite.rect.size.x + xCorrection) / 14f) - ((camsize * 2f * pixelPerUnit) / 7f) - ((player.transform.position.x - Xmin) * BGMoveX), camera.transform.position.y + ((spriteRenderer.sprite.rect.size.y + yCorrection) / 14f) - ((camsize * pixelPerUnit) / 7f) - ((player.transform.position.y - Ymin) * BGMoveY), 0f);
        BGObject.transform.position = cameraV3;
    }
}
