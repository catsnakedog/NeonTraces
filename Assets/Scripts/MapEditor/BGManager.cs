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
    GameObject BGObject;

    SpriteRenderer spriteRenderer;

    Action actionM;

    [SerializeField] private List<GameObject> BGs = new List<GameObject>();

    int stage;

    public float Xmax = 0;
    public float Xmin = 10000;
    public float Ymax = 0;
    public float Ymin = 10000;

    public float BGMoveX = 0;
    public float BGMoveY = 0;

    public float DefaultX = 0;
    public float DefaultY = 0;

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
        camera = GameObject.Find("MainCamera");
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
        BGMoveX = (spriteRenderer.sprite.rect.size.x / 4f - (Data.saveData.gameData.camsize * 4f * pixelPerUnit) / 7f) / (Xmax - Xmin);
        BGMoveY = (spriteRenderer.sprite.rect.size.y / 4f - (Data.saveData.gameData.camsize * 2f * pixelPerUnit) / 7f) / (Ymax - Ymin);
        DefaultX = camera.transform.position.x + (spriteRenderer.sprite.rect.size.x / 8f) - ((Data.saveData.gameData.camsize * 2f * pixelPerUnit) / 7f);
        DefaultY = camera.transform.position.y + (spriteRenderer.sprite.rect.size.y / 8f) - ((Data.saveData.gameData.camsize * pixelPerUnit) / 7f);
        BGObject = Instantiate(BGs[stage], new Vector3(DefaultX, DefaultY, 0f), Quaternion.identity);

        actionM += BGMoveSetting;
    }

    void BGMoveSetting()
    {
        Vector3 cameraV3 = new Vector3(camera.transform.position.x + (spriteRenderer.sprite.rect.size.x / 8f) - ((Data.saveData.gameData.camsize * 2f * pixelPerUnit) / 7f) - ((player.transform.position.x - Xmin) * BGMoveX), camera.transform.position.y + (spriteRenderer.sprite.rect.size.y / 8f) - ((Data.saveData.gameData.camsize * pixelPerUnit) / 7f) - ((player.transform.position.y - Ymin) * BGMoveY), 0f);
        BGObject.transform.position = cameraV3;
    }
}
