using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    DataManager Data;
    MapMake mapMake; // �׽�Ʈ�� ���� �ӽ÷� �߰� ���߿� �����

    public Action playerAction = null;
    [SerializeField] public float speed;
    [SerializeField] public float playerActionSpeed;
    [SerializeField] public float power;
    [SerializeField] private int stage;
    [SerializeField] public int crruentMoveDot;
    [SerializeField] private int moveDotSize;
    [SerializeField] private GameObject player;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;

    [SerializeField] private string className;
    [SerializeField] private bool isCountUp;
    [SerializeField] public bool BackOrFront;

    void Awake()
    {
        Data = DataManager.data;
        Data.saveData.gameData.player = player;
    }

    void Update()
    {
        playerAction?.Invoke();
    }

    void MoveBySpeed() // �÷��̾ endPoint�� �̵���Ŵ
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint�� �����Ͽ��°��� �Ǻ�
    {
        if (player.transform.position == endPoint)
        {
            playerAction -= MoveBySpeed;
            playerAction -= IsEndPoint;
            if(isCountUp)
            {
                crruentMoveDot++;
            }
            Invoke(className, 0f);
        }
    }

    void IsStartPoint() // �ǰ����� ���� �ڷ� �з����� startPoint�� �Ѿ�ٸ� �� ���� �̵������� ���� �̵��Ѵ�
    {
    }

    public void MoveStart() // �⺻���� ������, moveDots�� ���� �̵��Ѵ�, crreuntMoveDot�� ���������� �Էµ��ִٸ� ���߿� ����ߴٰ� �ٽ� �����ص� �������
    {
        moveDotSize = Data.saveData.mapData[stage].moveDots.Count;
        if (crruentMoveDot == moveDotSize-1)
        {
            Debug.Log("MoveEnd");
        }
        else
        {
            startPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
            endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
            speed = Data.saveData.mapData[stage].moveDots[crruentMoveDot].speed;
            MoveAToB("MoveStart", true);
        }
    }
    public void MoveAToB(string className, bool isCountUp) // A���� B���� �̵��ϴ� �Լ�, className���� �̵��� ������ �����ų �Լ��� ������ �� �ִ�
    {
        playerAction += MoveBySpeed;
        this.isCountUp = isCountUp;
        this.className = className;
        playerAction += IsEndPoint;
    }
    public void PlayerMoveBackOrFront(float power, bool BackOrFront) // �ǰݽ� or ���ݽ� �� �ڷ� �����̴� �Լ�, BackOrFront : true = �ڷ�, false = ������
    {
        Vector3 vDist = startPoint - endPoint;
        Vector3 vDir = vDist.normalized;
        float a;
        float b;
        if (BackOrFront)
        {
            endPoint = player.transform.position + (vDir * power);
            a = (player.transform.position - startPoint).magnitude;
            b = (player.transform.position - endPoint).magnitude;
        }
        else
        {
            a = (player.transform.position - endPoint).magnitude;
            endPoint = player.transform.position - (vDir * power);
            b = (player.transform.position - endPoint).magnitude;
        }
        playerAction = null;
        if (a < b) // startPoint�� �ʰ��ϴ� ���
        {
            if (crruentMoveDot == moveDotSize-1)
            {
                if (BackOrFront)
                {
                    endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot - 1].v3;
                    crruentMoveDot--;
                }
                else
                {
                    endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
                }
                speed = playerActionSpeed;
                MoveAToB("MoveStart", false);
            }
            else if(crruentMoveDot == 0)
            {
                if (BackOrFront)
                {
                    endPoint = startPoint;
                }
                else
                {
                    endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
                    crruentMoveDot++;
                }
                speed = playerActionSpeed;
                MoveAToB("MoveStart", false);
            }
            else
            {
                if(BackOrFront)
                {
                    endPoint = startPoint;
                    crruentMoveDot--;
                }
                else
                {
                    endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
                    crruentMoveDot++;
                }
                speed = playerActionSpeed;
                this.power = (b - a) / vDir.magnitude;
                this.BackOrFront = BackOrFront;
                MoveAToB("Rebound", false);
            }
        }
        else
        {
            speed = playerActionSpeed;
            MoveAToB("MoveStart", false);
        }
    }
    public void GameReSet() // ������ �ʱ�ȭ�Ѵ�
    {
        stage = Data.saveData.gameData.stage;
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerAction = null;
    }

    public void GameStart()
    {
        GameReSet();
        MoveStart();
    }

    public void GameStop() // ������ �����
    {
        Time.timeScale = 0;
    }

    public void GameResume() // ������ �ٽ� �����Ѵ�
    {
        Time.timeScale = 1;
    }

    public void GameBack() // �÷��̾��� ��ġ�� �ڷ� �ǵ�����
    {
        power = 10f;
        BackOrFront = true;
        playerActionSpeed = 10f;
        Rebound();
    }
    public void GameFront() // �÷��̾��� ��ġ�� �ڷ� �ǵ�����
    {
        power = 10f;
        BackOrFront = false;
        playerActionSpeed = 10f;
        Rebound();
    }
    public void Rebound() // ���ý� �и� ���� �׽�Ʈ
    {
        startPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
        if (crruentMoveDot == moveDotSize-1)
        {
            endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
        }
        else
        {
            endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
        }
        PlayerMoveBackOrFront(power, BackOrFront);
    }

    public void Test2() // �Ŀ� �и� ���� ����
    {
        playerActionSpeed = 1f;
        power = 1f;
        BackOrFront = true;
    }
}
