using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerAction : MonoBehaviour
{
    DataManager Data;

    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // �÷��̾ ���� or ��� ������ Ȯ���ϴ� ������
    [SerializeField] public bool isAction; // EnemySetting�� ��ȣ�ۿ��Ҷ� ����, ��� ������ �Ǻ��ϴ� ����
    [SerializeField] public bool isDelay; // ������ Ȯ�ο� ����
    [SerializeField] public bool isLongClick;
    [SerializeField] public bool isNextAttack;
    [SerializeField] private float attackDelay; // ����ݽ� ������
    [SerializeField] private float attackMotionTime; // ���� ���ϸ��̼� ����ð�
    [SerializeField] private float defenceMotionTime; // ��� ���ϸ��̼� ����ð�
    [SerializeField] private float longClickTime;
    [SerializeField] private float timeCount;

    [SerializeField] List<Sprite> effectSprites;
    [SerializeField] List<GameObject> effects;
    [SerializeField] GameObject defaultEffect;

    public Coroutine actionC;
    public Coroutine actionA;
    public Coroutine aniC;
    public Coroutine nextA;
    Coroutine effectC;

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
        EffectSetting();
    }

    public void EffectSetting()
    {
        GameObject temp;
        foreach(Sprite sprite in effectSprites)
        {
            temp = Instantiate(defaultEffect, Vector3.zero, Quaternion.identity);
            temp.GetComponent<SpriteRenderer>().sprite = sprite;
            temp.transform.SetParent(GameObject.Find("Player").transform.GetChild(3), false);
            effects.Add(temp);
            temp.SetActive(false);
        }
    }

    IEnumerator ShowEffect(int start, int end)
    {
        for (int i = start; i < end + 1; i++)
        {
            effects[i].SetActive(false);
        }

        for (int i = start; i < end + 1; i++)
        {
            effects[i].transform.position = DataManager.data.saveData.gameData.player.transform.position;
            StartCoroutine(Effect(effects[i]));
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator Effect(GameObject temp)
    {
        temp.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        temp.SetActive(false);
    }

    public void ActionReset()
    {
        isAttack = false;
        isDefence = false;
        isAction = false;
        isDelay = false;
        isLongClick = false;
        isNextAttack = false;
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
        actionC = StartCoroutine("LongAttackAction");
    }

    public void Death() // �÷��̾ ������ ����
    {
        GameOver();
    }

    IEnumerator AttackAction() // Attack ���� ����
    {
        if(aniC != null)
        {
            StopCoroutine(aniC);
        }
        if(nextA != null)
        {
            StopCoroutine(nextA);
        }
        if (effectC != null)
        {
            StopCoroutine(effectC);
        }
        if(isNextAttack)
        {
            aniC = StartCoroutine(CallAni("AttackRight", attackMotionTime));
            isNextAttack=false;
        }
        else
        {
            aniC = StartCoroutine(CallAni("AttackLeft", attackMotionTime));
            nextA = StartCoroutine(NextAttack(2f));
        }
        effectC = StartCoroutine(ShowEffect(0, 5));
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
    IEnumerator DefenceAction() // Defence ���� ����
    {
        if (aniC != null)
        {
            StopCoroutine(aniC);
        }
        aniC = StartCoroutine(CallAni("Defence", defenceMotionTime));
        isDefence = true;
        isAction = true;
        isDelay = true;
        yield return new WaitForSeconds(defenceMotionTime);
        isAction = false;
        isDefence = false;
        yield return new WaitForSeconds(attackDelay);
        isDelay = false;
    }
    IEnumerator LongAttackAction() // Defence ���� ����
    {
        if (aniC != null)
        {
            StopCoroutine(aniC);
        }
        aniC = StartCoroutine(CallAni("DragAttack", attackMotionTime));
        isLongClick = false;
        isAttack = true;
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
        if (isLongClick)
        {
            LongAttack();
        }
        if (!isDelay)
        {
            if(!isLongClick)
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
            isDelay = true;
            isAction = true;
            playerAction = null;
            isLongClick = true;
            actionA = StartCoroutine(afterImage.AfterImageSetting(Data.saveData.gameData.player));
            animaiton.SetAnimation("Drag");
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

    IEnumerator CallAni(string Name, float time)
    {
        animaiton.SetAnimation(Name);
        yield return new WaitForSeconds(time);
        animaiton.SetAnimation("Run");
    }

    IEnumerator NextAttack(float time)
    {
        isNextAttack = true;
        yield return new WaitForSeconds(time);
        isNextAttack = false;
    }

    void Effect()
    {

    }
}
