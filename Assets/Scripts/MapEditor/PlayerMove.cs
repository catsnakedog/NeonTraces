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

    void IsStartPoint() // 피격으로 인해 뒤로 밀려날시 startPoint를 넘어간다면 그 전의 이동라인을 따라 이동한다
    {
    }

    public void MoveStart() // 기본적인 움직임, moveDots를 따라서 이동한다, crreuntMoveDot만 정상적으로 입력돼있다면 도중에 취소했다가 다시 시작해도 상관없다
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
        if (a < b) // startPoint를 초과하는 경우
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
    public void GameReSet() // 세팅을 초기화한다
    {
        player.transform.position = Data.saveData.mapData[stage].moveDots[0].v3;
        crruentMoveDot = 0;
        Time.timeScale = 1;
        playerAction = null;
    }

    public void GameStop() // 게임을 멈춘다
    {
        Time.timeScale = 0;
    }

    public void GameResume() // 게임을 다시 시작한다
    {
        Time.timeScale = 1;
    }

    public void GameBack() // 플레이어의 위치를 뒤로 되돌린다
    {
        power = 10f;
        BackOrFront = true;
        playerActionSpeed = 10f;
        Test();
    }
    public void Test() // 어택시 밀림 판정 테스트
    {
        startPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot].v3;
        endPoint = Data.saveData.mapData[stage].moveDots[crruentMoveDot + 1].v3;
        PlayerMoveBackOrFront(power, BackOrFront);
    }

    public void Test2() // 파워 밀림 방향 세팅
    {
        playerActionSpeed = 1f;
        power = 1f;
        BackOrFront = true;
    }
}
