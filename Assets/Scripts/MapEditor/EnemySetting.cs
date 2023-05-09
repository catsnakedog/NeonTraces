using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemySetting : MonoBehaviour
{
    [SerializeField] public bool isActive = true;
    [SerializeField] public bool isCountUp;
    [SerializeField] public int type;
    [SerializeField] public int index;
    [SerializeField] public int startDot;
    [SerializeField] public int currentDot;
    [SerializeField] public int endDot;
    [SerializeField] public float speed;
    [SerializeField] public float power; //0이면 포인트 끝까지 이동
    [SerializeField] public float DefaultSpeed;
    [SerializeField] public float DefaultPower;
    [SerializeField] public string className;
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
        if(playerAction.isDefence)
        {
            PlayerRebound();
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void Defence() // enemy가 방어 상태일때
    {
        if (playerAction.isAttack)
        {
            EnemyRebound();
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void PatternClear() // 성공적으로 판정에 성공 했을때 player 행동 쿨타임 초기화
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
            isActive = true;
            cnt++;
        }
    }
    void Death() // 패턴이 전부 소모시 사망
    {
        enemyAction = null;
    }

    public void EnemyMoveStart() // startDot -> 적이 움직이는 방향 반대방향에서 가장 가까운 점, endPoint -> 적이 어떤 점까지 움직이는지
    {
        if(power == 0) // 적이 특정 지점까지 이동하는 이펙트, power 0으로 세팅해주면 된다
        {
            if (startDot == endDot)
            {
                Debug.Log("적이동 완료");
            }
            else if (startDot > endDot)
            {
                currentDot = startDot - 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", false);
            }
            else
            {
                currentDot = startDot + 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", true);
            }
        }
        else // 적이 단순히 이동하는게 아니라 플레이어에게 공격당하고 밀릴때 판정
        {
            startPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
            endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot+1].v3;
            Vector3 vDist = endPoint - startPoint;
            Vector3 vDir = vDist.normalized;
            Vector3 tempEndV = endPoint;
            endPoint = gameObject.transform.position + (vDir * power);
            float a = (gameObject.transform.position - tempEndV).magnitude;
            float b = (gameObject.transform.position - endPoint).magnitude;
            if (b > a)
            {
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[playerMove.crruentMoveDot+1].v3;
                MoveAtoB("", true);
            }
            else
            {
                MoveAtoB("", true);
            }
        }
    }

    void MoveAtoB(string className, bool isCountUp) // 적을 이동 시키기 위한 함수
    {
        this.className = className;
        this.isCountUp = isCountUp;
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
            if(isCountUp)
            {
                startDot++;
            }
            else
            {
                startDot--;
            }
            if(className != "")
            {
                Invoke(className, 0f);
            }
            Data.saveData.gameData.enemyInfo[index].x = gameObject.transform.position.x;
        }
    }

    void PlayerRebound()
    {
        playerMove.power = 2f;
        playerMove.playerActionSpeed = 20;
        playerMove.BackOrFront = true;
        playerMove.Rebound();
    }

    void EnemyRebound()
    {
        power = DefaultPower;
        speed = DefaultSpeed;
        startDot = playerMove.crruentMoveDot;
        endDot = playerMove.crruentMoveDot+1;
        EnemyMoveStart();
    }

    ~EnemySetting() // 소멸자
    {
        enemyAction = null;
    }
}
