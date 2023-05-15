using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    DataManager Data;
    MapMake mapMake; // 테스트를 위해 임시로 추가 나중에 지울것
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
        Data = DataManager.data;
        player = GameObject.Find("Player");
        Data.saveData.gameData.player = player;
        mapMake = GameObject.Find("MapMaker").GetComponent<MapMake>();
        playerAnimation = gameObject.GetComponent<PlayerAnimaiton>();
        playerAnimation.AniSet();
        playerActionS = gameObject.GetComponent<PlayerAction>();
    }

    void Update()
    {
        playerAction?.Invoke();
    }

    void MoveBySpeed() // 플레이어를 endPoint로 이동시킴
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint에 도달하였는가를 판별
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

    public void MoveStart() // 기본적인 움직임, moveDots를 따라서 이동한다, crreuntMoveDot만 정상적으로 입력돼있다면 도중에 취소했다가 다시 시작해도 상관없다
    {
        moveDotSize = Data.saveData.mapData[stage].moveDots.Count;
        if (crruentMoveDot == moveDotSize-1)
        {
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
    public void MoveAToB(string className, bool isCountUp) // A부터 B까지 이동하는 함수, className으로 이동이 끝나고 실행시킬 함수를 지정할 수 있다
    {
        playerAction += MoveBySpeed;
        this.isCountUp = isCountUp;
        this.className = className;
        playerAction += IsEndPoint;
    }
    public void PlayerMoveBackOrFront(float power, bool BackOrFront) // 피격시 or 공격시 앞 뒤로 움직이는 함수, BackOrFront : true = 뒤로, false = 앞으로
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
        if (a < b) // startPoint를 초과하는 경우
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
    public void GameReSet() // 세팅을 초기화한다
    {
        stage = Data.saveData.gameData.stage;
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerAction = null;
    }

    public void GameStart() // 게임 시작
    {
        GameReSet();
        MoveStart();
    }

    public void Rebound() // 어택시 밀림 판정 테스트
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

    public void BezierMove(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float time) // 곡선 이동
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
