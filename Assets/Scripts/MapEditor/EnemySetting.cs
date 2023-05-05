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
            Debug.Log("버그야 버그");
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
        Debug.Log("적 사망");
    }

    void MoveAtoB()
    {
        enemyAction += MoveBySpeed;
        enemyAction += IsEndPoint;
    }

    void MoveBySpeed() // 플레이어를 endPoint로 이동시킴
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
}
