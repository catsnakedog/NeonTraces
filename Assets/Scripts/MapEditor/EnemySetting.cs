using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemySetting : MonoBehaviour
{
    [SerializeField] public bool isActive = true;
    [SerializeField] public int type;
    [SerializeField] public float speed;
    [SerializeField] public float power;
    [SerializeField] public List<int> pattern; // 0 공격, 1 방어, 2...(특수액션)

    int cnt;
    GameObject player;
    DataManager Data;

    Vector3 startPoint;
    Vector3 endPoint;

    Action enemyAction = null;
    PlayerAction playerAction;
    PlayerMove playerMove;
    void Start()
    {
        Data = DataManager.data;
        player = Data.saveData.gameData.player;
        playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        playerMove = GameObject.Find("InGameManager").GetComponent<PlayerMove>();
        cnt = 0;
    }

    void Update()
    {
        enemyAction?.Invoke();   
    }

    private void OnTriggerStay2D(Collider2D collision) // player와 충돌시 판정
    {
        if(isActive)
        {
            if (collision.name == "Life") // 플레이어 본체와 충돌시 사망
            {
                playerAction.Death();
            }
            if (collision.name == "Action") // 공격, 방어 범위와 충돌시 판정에 들어감
            {
                if (playerAction.isAction)
                {
                    EnemyPattern();
                }
            }
        }
    }

    void EnemyPattern() // 공격, 방어 성공 판정 확인
    {
        if (pattern[cnt] == 0)
        {
            Attack();
        }
        else if (pattern[cnt] == 1)
        {
            Defence();
        }
        else
        {
            Debug.Log("버그야 버그");
        }
    }

    void Attack() // enemy가 공격 상태일때
    {
        Debug.Log("a");
        if(playerAction.isDefence)
        {
            patternClear();
            playerMove.power = 1;
            playerMove.playerActionSpeed = 10;
            playerMove.BackOrFront = true;
            playerMove.Rebound();
        }
        else
        {
            playerAction.Death();
        }
    }

    void Defence() // enemy가 방어 상태일때
    {
        Debug.Log("b");
        if (playerAction.isAttack)
        {
            patternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void patternClear() // 성공적으로 판정에 성공 했을때 player 행동 쿨타임 초기화
    {
        isActive = false;
        playerAction.isDelay = false;
        playerAction.isAction = false;
        playerAction.isDefence = false;
        playerAction.isAttack = false;
        playerAction.StopCoroutine(playerAction.actionC);
        if (cnt == pattern.Count - 1)
        {
            Death();
        }
        else
        {
            if (pattern[cnt] == 1)
            {
                Vector3 vDist = gameObject.transform.position - player.transform.position;
                Vector3 vDir = vDist.normalized;
                endPoint = gameObject.transform.position + vDir*power;
                MoveAtoB();
            }
            cnt++;
        }
    }
    void Death() // 패턴이 전부 소모시 사망
    {
        Debug.Log("적 사망");
    }

    void MoveAtoB() // 적을 이동 시키기 위한 함수
    {
        enemyAction += MoveBySpeed;
        enemyAction += IsEndPoint;
    }

    void MoveBySpeed() // enemy를 endPoint로 이동시킴
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint에 도달하였는가를 판별
    {
        if (gameObject.transform.position == endPoint)
        {
            enemyAction -= MoveBySpeed;
            enemyAction -= IsEndPoint;
            isActive = true;
        }
    }

    ~EnemySetting() // 소멸자
    {
        enemyAction = null;
    }
}
