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

    void Defence() // enemy�� ��� �����϶�
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

    void patternClear() // ���������� ������ ���� ������ player �ൿ ��Ÿ�� �ʱ�ȭ
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
    void Death() // ������ ���� �Ҹ�� ���
    {
        Debug.Log("�� ���");
    }

    void MoveAtoB() // ���� �̵� ��Ű�� ���� �Լ�
    {
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
            isActive = true;
        }
    }

    ~EnemySetting() // �Ҹ���
    {
        enemyAction = null;
    }
}
