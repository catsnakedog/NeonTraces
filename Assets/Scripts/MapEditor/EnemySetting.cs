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
    [SerializeField] public float power; //0�̸� ����Ʈ ������ �̵�
    [SerializeField] public float DefaultSpeed;
    [SerializeField] public float DefaultPower;
    [SerializeField] public string className;
    [SerializeField] public List<int> pattern; // 0 ����, 1 ���, 2...(Ư���׼�)

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

    private void OnTriggerStay2D(Collider2D collision) // player�� �浹�� ����
    {
        if(isActive)
        {
            if (collision.name == "Life") // �÷��̾� ��ü�� �浹�� ���
            {
                playerAction.Death();
            }
            if (collision.name == "Action") // ����, ��� ������ �浹�� ������ ��
            {
                if (playerAction.isAction)
                {
                    EnemyPattern();
                }
            }
        }
    }

    void EnemyPattern() // ����, ��� ���� ���� Ȯ��
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
            Debug.Log("���׾� ����");
        }
    }

    void Attack() // enemy�� ���� �����϶�
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

    void Defence() // enemy�� ��� �����϶�
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

    void PatternClear() // ���������� ������ ���� ������ player �ൿ ��Ÿ�� �ʱ�ȭ
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
    void Death() // ������ ���� �Ҹ�� ���
    {
        enemyAction = null;
    }

    public void EnemyMoveStart() // startDot -> ���� �����̴� ���� �ݴ���⿡�� ���� ����� ��, endPoint -> ���� � ������ �����̴���
    {
        if(power == 0) // ���� Ư�� �������� �̵��ϴ� ����Ʈ, power 0���� �������ָ� �ȴ�
        {
            if (startDot == endDot)
            {
                Debug.Log("���̵� �Ϸ�");
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
        else // ���� �ܼ��� �̵��ϴ°� �ƴ϶� �÷��̾�� ���ݴ��ϰ� �и��� ����
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

    void MoveAtoB(string className, bool isCountUp) // ���� �̵� ��Ű�� ���� �Լ�
    {
        this.className = className;
        this.isCountUp = isCountUp;
        enemyAction += MoveBySpeed;
        enemyAction += IsEndPoint;
    }

    void MoveBySpeed() // enemy�� endPoint�� �̵���Ŵ
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint�� �����Ͽ��°��� �Ǻ�
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

    ~EnemySetting() // �Ҹ���
    {
        enemyAction = null;
    }
}
