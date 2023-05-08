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
     * 1 jump down 이동 / 미구현
     * 2 속도 점진적 증가, 감소 / 미구현
     * 3 컷씬 / 미구현
     * 4 적이동 / 미구현
     * 5 소리 / 미구현
     * 6 카메라 액션 / 미구현
     * 7 대쉬 / 구현
     * 8 시간 / 구현
     * 10 에니메이션 변경 / 미구현
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
