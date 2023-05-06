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

    Action playerAction = null;
    [SerializeField] private float speed;
    [SerializeField] private float playerActionSpeed;
    [SerializeField] private float power;
    [SerializeField] private int stage;
    [SerializeField] private int crruentMoveDot;
    [SerializeField] private int MoveDotSize;
    [SerializeField] private GameObject player;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;

    [SerializeField] private string className;
    [SerializeField] private bool isCountUp;
    [SerializeField] private bool BackOrFront;

    void Start()
    {
        Data = DataManager.data;
        Data.saveData.gameData.player = player;
        stage = Data.saveData.gameData.stage;
        crruentMoveDot = 0;
        player.transform.position = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
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
        MoveDotSize = Data.saveData.mapData[stage].moveDots.Count;
        if(crruentMoveDot == MoveDotSize-1)
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
        if (BackOrFront)
        {
            endPoint = player.transform.position + (vDir * power);
        }
        else
        {
            endPoint = player.transform.position + (vDir * power * -1);
        }
        float a = (player.transform.position - startPoint).magnitude;
        float b = (player.transform.position - endPoint).magnitude;
        if (a < b) // startPoint�� �ʰ��ϴ� ���
        {
            if(crruentMoveDot == 0)
            {
                playerAction = null;
                endPoint = startPoint;
                speed = playerActionSpeed;
                MoveAToB("MoveStart", false);
            }
            else
            {
                playerAction = null;
                endPoint = startPoint;
                speed = playerActionSpeed;
                this.power = (b - a) / vDir.magnitude;
                this.BackOrFront = BackOrFront;
                crruentMoveDot--;
                MoveAToB("Test", false);
            }
        }
        else
        {
            playerAction = null;
            speed = playerActionSpeed;
            MoveAToB("MoveStart", false);
        }
    }
    public void GameReSet() // ������ �ʱ�ȭ�Ѵ�
    {
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerAction = null;
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
        Test();
    }
    public void Test() // ���ý� �и� ���� �׽�Ʈ
    {
        startPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
        endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
        PlayerMoveBackOrFront(power, BackOrFront);
    }

    public void Test2() // �Ŀ� �и� ���� ����
    {
        playerActionSpeed = 1f;
        power = 1f;
        BackOrFront = true;
    }
}
