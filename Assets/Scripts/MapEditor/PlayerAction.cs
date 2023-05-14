using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    DataManager Data;

    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // 플레이어가 공격 or 방어 중인지 확인하는 변수들
    [SerializeField] public bool isAction; // EnemySetting과 상호작용할때 공격, 방어 중인지 판별하는 변수
    [SerializeField] public bool isDelay; // 딜레이 확인용 변수
    [SerializeField] private float attackDelay; // 헛공격시 딜레이
    [SerializeField] private float attackMotionTime; // 어택 에니메이션 실행시간
    [SerializeField] private float defenceMotionTime; // 방어 에니메이션 실행시간

    public Coroutine actionC;
    public Coroutine actionA;

    AfterImage afterImage;

    void Start()
    {
        Data = DataManager.data;
        afterImage = transform.GetComponent<AfterImage>();
    }

    public void Attack() // 플레이어 공격
    {
        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence() // 플레이어 방어
    {
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }

    public void Death() // 플레이어가 죽을시 실행
    {
        GameOver();
    }

    IEnumerator AttackAction() // Attack 관련 세팅
    {
        actionA = StartCoroutine(afterImage.AfterImageSetting(Data.saveData.gameData.player));
        isAttack = true;
        isAction = true;
        isDelay = true;
        yield return new WaitForSeconds(attackMotionTime);
        isAction = false;
        isAttack = false;
        yield return new WaitForSeconds(attackDelay);
        isDelay = false;
        StopCoroutine(actionA);
    }
    IEnumerator DefenceAction() // Defence 관련 세팅
    {
        isDefence = true;
        isAction = true;
        isDelay = true;
        yield return new WaitForSeconds(defenceMotionTime);
        isAction = false;
        isDefence = false;
        yield return new WaitForSeconds(attackDelay);
        isDelay = false;
    }

    void GameOver() // 게임오버시 실행되는 함수, 관련된 내용은 여기 안에다가 작업하면 된다
    {
        Time.timeScale = 0f;
        Debug.Log("게임오버");
    }
}
