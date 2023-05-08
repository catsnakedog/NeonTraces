using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDotSetting : MonoBehaviour
{
    DataManager Data;
    PlayerMove playerMove;
    PlayerAction playerAction;
    GameObject player;
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

    public void Event1()
    {

    }
}
