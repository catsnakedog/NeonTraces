using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDotSetting : MonoBehaviour
{
    DataManager Data;
    PlayerMove playerMove;
    PlayerAction playerAction;

    GameObject player;
    public EventTypeInfo eventTypeInfo;

    public int type;
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("adasdasd");
        if(collision.name == "Life")
        {
            if (type == 0) Event0();
            else if (type == 1) Event1();
            else if (type == 2) Event2();
            else if (type == 3) Event3();
            else if (type == 4) Event4();
            else if (type == 5) Event5();
            else if (type == 6) Event6();
            else if (type == 7) Event7();
            else if (type == 8) Event8();
            else if (type == 9) Event9();
        }
    }

    public void Event0()
    {
    }
    public void Event1()
    {
    }
    public void Event2()
    {
    }
    public void Event3()
    {
    }
    public void Event4()
    {
    }
    public void Event5()
    {
    }
    public void Event6()
    {
    }
    public void Event7()
    {
    }
    public void Event8()
    {
    }
    public void Event9()
    {
    }
}
