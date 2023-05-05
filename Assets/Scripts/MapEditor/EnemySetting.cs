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
    void Start()
    {
        Data = DataManager.data;
        player = Data.saveData.gameData.player;
        playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        cnt = 0;
    }

    void Update()
    {
        enemyAction?.Invoke();   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isActive)
        {
            if (collision.name == "Life")
            {
                playerAction.Death();
            }
            if (collision.name == "Action")
            {
                if (playerAction.isAction)
                {
                    EnemyPattern();
                }
            }
        }
    }

    void EnemyPattern()
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

    void Attack()
    {
        Debug.Log("a");
        if(playerAction.isDefence)
        {
            patternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void Defence()
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

    void patternClear()
    {
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
                isActive = false;
                Vector3 vDist = gameObject.transform.position - player.transform.position;
                Vector3 vDir = vDist.normalized;
                endPoint = gameObject.transform.position + vDir*power;
                MoveAtoB();
            }
            cnt++;
        }
    }
    void Death()
    {
        isActive = false;
        Debug.Log("�� ���");
    }

    void MoveAtoB()
    {
        enemyAction += MoveBySpeed;
        enemyAction += IsEndPoint;
    }

    void MoveBySpeed() // �÷��̾ endPoint�� �̵���Ŵ
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
}
