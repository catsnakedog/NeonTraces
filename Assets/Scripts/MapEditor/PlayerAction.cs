using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    DataManager Data;

    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // �÷��̾ ���� or ��� ������ Ȯ���ϴ� ������
    [SerializeField] public bool isAction; // EnemySetting�� ��ȣ�ۿ��Ҷ� ����, ��� ������ �Ǻ��ϴ� ����
    [SerializeField] public bool isDelay; // ������ Ȯ�ο� ����
    [SerializeField] public bool isLongClick;
    [SerializeField] private float attackDelay; // ����ݽ� ������
    [SerializeField] private float attackMotionTime; // ���� ���ϸ��̼� ����ð�
    [SerializeField] private float defenceMotionTime; // ��� ���ϸ��̼� ����ð�
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

    public void Attack() // �÷��̾� ����
    {

        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence() // �÷��̾� ���
    {
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }
    public void LongAttack()
    {
        if (!isDelay) actionC = StartCoroutine("LongAttackAction");
    }

    public void Death() // �÷��̾ ������ ����
    {
        GameOver();
    }

    IEnumerator AttackAction() // Attack ���� ����
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
    IEnumerator DefenceAction() // Defence ���� ����
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
    IEnumerator LongAttackAction() // Defence ���� ����
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
            Debug.Log("��Ŭ�� ���� ���ϸ��̼� ����"); // ���ϸ��̼� �߰��Ǹ� ����ٰ� �߰�
        }
    }

    void GameOver() // ���ӿ����� ����Ǵ� �Լ�, ���õ� ������ ���� �ȿ��ٰ� �۾��ϸ� �ȴ�
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
        Debug.Log("���ӿ���");
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
