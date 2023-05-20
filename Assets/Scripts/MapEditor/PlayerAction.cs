using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    DataManager Data;

    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // 플레이어가 공격 or 방어 중인지 확인하는 변수들
    [SerializeField] public bool isAction; // EnemySetting과 상호작용할때 공격, 방어 중인지 판별하는 변수
    [SerializeField] public bool isDelay; // 딜레이 확인용 변수
    [SerializeField] public bool isLongClick;
    [SerializeField] private float attackDelay; // 헛공격시 딜레이
    [SerializeField] private float attackMotionTime; // 어택 에니메이션 실행시간
    [SerializeField] private float defenceMotionTime; // 방어 에니메이션 실행시간
    [SerializeField] private float longClickTime;
    [SerializeField] private float timeCount;

    public Coroutine actionC;
    public Coroutine actionA;

    AfterImage afterImage;

    Action playerAction;
    public PlayerAnimaiton animaiton;

    void Start()
    {
        isAttack = false;
        isDefence = false;
        isAction = false;
        isDelay = false;
        isLongClick = false;
        timeCount = 0;
    }

    public void ActionReset()
    {
        isAttack = false;
        isDefence = false;
        isAction = false;
        isDelay = false;
        isLongClick = false;
        timeCount = 0;
    }

    public void StartSetting()
    {
        Data = DataManager.data;
        afterImage = transform.GetComponent<AfterImage>();
        animaiton = transform.GetComponent<PlayerAnimaiton>();
    }

    void Update()
    {
        playerAction?.Invoke();
    }

    public void Attack() // 플레이어 공격
    {

        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence() // 플레이어 방어
    {
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }
    public void LongAttack()
    {
        if (!isDelay) actionC = StartCoroutine("LongAttackAction");
    }

    public void Death() // 플레이어가 죽을시 실행
    {
        GameOver();
    }

    IEnumerator AttackAction() // Attack 관련 세팅
    {
        animaiton.SetAnimation("Attack");
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
        animaiton.SetAnimation("Run");
    }
    IEnumerator DefenceAction() // Defence 관련 세팅
    {
        animaiton.SetAnimation("Defence");
        isDefence = true;
        isAction = true;
        isDelay = true;
        yield return new WaitForSeconds(defenceMotionTime);
        isAction = false;
        isDefence = false;
        yield return new WaitForSeconds(attackDelay);
        isDelay = false;
        animaiton.SetAnimation("Run");
    }
    IEnumerator LongAttackAction() // Defence 관련 세팅
    {
        isLongClick = false;
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

    public void ButtonDown()
    {
        if (!isDelay)
        {
            timeCount = 0;
            isLongClick = false;
            playerAction += TimeCount;
            playerAction += IsLongClick;
        }
    }
    public void ButtonUp()
    {
        playerAction = null;
        if(!isDelay)
        {
            if (isLongClick)
            {
                LongAttack();
            }
            else
            {
                Attack();
            }
        }
    }

    void TimeCount()
    {
        timeCount += Time.deltaTime;
    }

    void IsLongClick()
    {
        if(timeCount >= longClickTime)
        {
            playerAction = null;
            isLongClick = true;
            actionA = StartCoroutine(afterImage.AfterImageSetting(Data.saveData.gameData.player));
            Debug.Log("롱클릭 공격 에니메이션 실행"); // 에니메이션 추가되면 여기다가 추가
        }
    }

    void GameOver() // 게임오버시 실행되는 함수, 관련된 내용은 여기 안에다가 작업하면 된다
    {
        if (actionC != null)
        {
            StopCoroutine(actionC);
        }
        if (actionA != null)
        {
            StopCoroutine(actionA);
        }
        playerAction = null;
        Time.timeScale = 0f;
        Debug.Log("게임오버");
    }

    public void PlayerActionReset()
    {
        if(actionC != null)
        {
            StopCoroutine(actionC);
        }
        if (actionA != null)
        {
            StopCoroutine(actionA);
        }
        playerAction = null;
    }
}
