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
    PlayerAnimaiton playerAnimation;
    PlayerAction playerActionS;

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

    void Start()
    {
    }

    public void StartSetting()
    {
        Data = DataManager.data;
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerAnimation = gameObject.GetComponent<PlayerAnimaiton>();
        playerActionS = gameObject.GetComponent<PlayerAction>();
    }

    void FixedUpdate()
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

    public void MoveStart() // �⺻���� ������, moveDots�� ���� �̵��Ѵ�, crreuntMoveDot�� ���������� �Էµ��ִٸ� ���߿� ����ߴٰ� �ٽ� �����ص� �������
    {
        if(crruentMoveDot == 0) SoundManager.sound.Play("BGM_0" + (Data.saveData.gameData.stage + 1).ToString());
        moveDotSize = Data.saveData.mapData[stage].moveDots.Count;
        if (crruentMoveDot == moveDotSize-1)
        {
            playerAnimation.SetAnimation("Idle");
            Debug.Log("MoveEnd");
        }
        else
        {
            if(!playerActionS.isDelay)
            {
                playerAnimation.SetAnimation("Run");
            }
            startPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
            endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
            if(startPoint.x > endPoint.x)
            {
                player.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                player.transform.localScale = new Vector3(1f, 1f, 1f);
            }
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
        SoundManager.sound.Play("");
        stage = Data.saveData.gameData.stage;
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        playerAnimation.SetAnimation("Idle");
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerActionS.ActionReset();
        playerAction = null;
    }

    public void GameStart() // ���� ����
    {
        stage = Data.saveData.gameData.stage;
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerActionS.ActionReset();
        playerAction = null;
        playerAnimation.SetAnimation("Run");
        MoveStart();
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

    public void BezierMove(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float time) // � �̵�
    {
        Vector3 A = Vector3.Lerp(v1, v2, time);
        Vector3 B = Vector3.Lerp(v2, v3, time);
        Vector3 C = Vector3.Lerp(v3, v4, time);
        Vector3 D = Vector3.Lerp(A, B, time);
        Vector3 E = Vector3.Lerp(B, C, time);
        Vector3 F = Vector3.Lerp(D, E, time);

        player.transform.position = F;
    }
}
