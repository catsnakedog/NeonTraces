using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventDotSetting : MonoBehaviour
{
    DataManager Data;
    PlayerMove playerMove;
    PlayerAction playerAction;
    CutSceneManager cutSceneManager;

    GameObject player;
    public EventTypeInfo eventTypeInfo;

    Coroutine eventCoroutine;
    Action eventAction;

    public int type;
    public float time = 0;
    /*
     * 1 jump down �̵� / �̱���
     * 2 �ӵ� ������ ����, ���� / �̱���
     * 3 �ƾ� / �̱���
     * 4 ���̵� / �̱���
     * 5 �Ҹ� / �̱���
     * 6 ī�޶� �׼� / �̱���
     * 7 �뽬 / ����
     * 8 �ð� / ����
     * 10 ���ϸ��̼� ���� / �̱���
     */

    void Start()
    {
        Data = DataManager.data;
        player = Data.saveData.gameData.player;
        playerMove = GameObject.Find("InGameManager").GetComponent<PlayerMove>();
        playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        cutSceneManager = GameObject.Find("InGameManager").GetComponent<CutSceneManager>();
    }

    void Update()
    {
        eventAction?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StopAllCoroutines();
        if(collision.name == "Life")
        {
            eventCoroutine = StartCoroutine("Event"+type);
        }
    }

    IEnumerator Event0() // �÷��̾� �̵� (jump, down)
    {
        playerMove.playerAction = null;
        time = 0;
        eventAction += BezierSetting;
        yield return new WaitForSeconds(eventTypeInfo.type0.time);
        eventAction -= BezierSetting;
        playerMove.crruentMoveDot = eventTypeInfo.type0.nextMoveDot;
        playerMove.MoveStart();
        gameObject.SetActive(false);
    }
    IEnumerator Event1() // �ӵ� ������ ����
    {
        eventAction += UpSpeed;
        yield return new WaitForSeconds(eventTypeInfo.type1.changeTime);
        eventAction -= UpSpeed;
        gameObject.SetActive(false);
    }
    IEnumerator Event2() // �ƾ�
    {
        playerMove.playerAction = null;
        cutSceneManager.Invoke("CutScene"+eventTypeInfo.type2.cutSceneNumber, 0f);
        yield return new WaitForSeconds(3f);
        playerMove.MoveStart();
        gameObject.SetActive(false);
    }
    IEnumerator Event3() // startDot ���� �����̴� ���� �ݴ��� ���� ����� ��, endDot ���� ������ �����̴°�(moveDot����)
    {
        EnemySetting enemy;
        enemy = Data.saveData.gameData.enemyInfo[eventTypeInfo.type3.index].enemy.GetComponent<EnemySetting>();
        enemy.power = 0;
        enemy.speed = eventTypeInfo.type3.speed;
        enemy.startDot = eventTypeInfo.type3.startDot;
        enemy.endDot = eventTypeInfo.type3.endDot;
        enemy.EnemyMoveStart();
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event4() // ����(�̱���)
    {
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event5() // ī�޶� �׼�(�̱���)
    {
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event6() // �뽬
    {
        playerMove.playerActionSpeed = eventTypeInfo.type6.speed;
        playerMove.power = eventTypeInfo.type6.power;
        playerMove.BackOrFront = false;
        playerMove.Rebound();
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event7() // �ð� ����
    {
        Time.timeScale = eventTypeInfo.type7.timeScale;
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event8() // ���ϸ��̼� ���� (�̱���)
    {
        yield return new WaitForSeconds(0f);
        gameObject.SetActive(false);
    }
    IEnumerator Event9() // stopTIme���� �÷��̾ �����(TimeScale != 0)
    {
        playerMove.playerAction = null;
        yield return new WaitForSeconds(eventTypeInfo.type9.stopTime);
        playerMove.MoveStart();
        gameObject.SetActive(false);
    }

    void UpSpeed() // Event1 ���� �Լ�, speed�� ���������� ������Ŵ
    {
        playerMove.speed += (eventTypeInfo.type1.upSpeed * Time.deltaTime) / eventTypeInfo.type1.changeTime;
    }

    void BezierSetting()
    {
        time += Time.deltaTime / eventTypeInfo.type0.time;
        playerMove.BezierMove(gameObject.transform.position, eventTypeInfo.type0.pointDot1, eventTypeInfo.type0.pointDot2, Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[eventTypeInfo.type0.nextMoveDot].v3, time);
    }
}