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
    [SerializeField] public bool isDefence; // 플레이어가 공격 or 방어 중인지 확인하는 변수들
    [SerializeField] public bool isAction; // EnemySetting과 상호작용할때 공격, 방어 중인지 판별하는 변수
    [SerializeField] public bool isDelay; // 딜레이 확인용 변수
    [SerializeField] public bool isLongClick;
    [SerializeField] public bool isNextAttack;
    [SerializeField] public bool isGameOver;
    [SerializeField] private float attackDelay; // 헛공격시 딜레이
    [SerializeField] private float attackMotionTime; // 어택 에니메이션 실행시간
    [SerializeField] private float defenceMotionTime; // 방어 에니메이션 실행시간
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
    PlayerMove playerMove;

    Action playerAction;
    public PlayerAnimaiton animaiton;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();

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

    public void ShowEffect2(int start, int end, Vector3 pos)
    {
        StartCoroutine(ShowEffect(start, end, pos));
    }

    public IEnumerator ShowEffect(int start, int end, Vector3 pos)
    {
        for (int i = start; i < end + 1; i++)
        {
            effects[i].transform.position = DataManager.data.saveData.gameData.player.transform.position;
            if (effects[i].activeSelf)
            {
                GameObject tempObj = Instantiate(effects[i], transform.position, Quaternion.identity);
                tempObj.SetActive(false);
                StartCoroutine(Effect(tempObj, true, pos));
            }
            else
            {
                StartCoroutine(Effect(effects[i], false, pos));
            }
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator Effect(GameObject temp, bool isIstance, Vector3 pos)
    {
        temp.transform.position += pos;
        temp.SetActive(true);
        yield return new WaitForSeconds(0.07f);
        temp.SetActive(false);
        if(isIstance)
        {
            Destroy(temp);
        }
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
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Defence();
        }
        playerAction?.Invoke();
    }

    public void Attack() // 플레이어 공격
    {
        if (animaiton.crruentAni == "Jump") return;
        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence() // 플레이어 방어
    {
        if (animaiton.crruentAni == "Jump") return;
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }
    public void LongAttack()
    {
        actionC = StartCoroutine("LongAttackAction");
    }

    public void Death() // 플레이어가 죽을시 실행
    {
        if (aniC != null)
        {
            StopCoroutine(aniC);
        }
        aniC = StartCoroutine(CallAni("Down", 0.74f));
        GameOver();
    }

    IEnumerator AttackAction() // Attack 관련 세팅
    {
        if(aniC != null)
        {
            StopCoroutine(aniC);
        }
        if(nextA != null)
        {
            StopCoroutine(nextA);
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
        SoundManager.sound.Play("main_attack" + UnityEngine.Random.Range(1, 3).ToString());
        effectC = StartCoroutine(ShowEffect(0, 5, new Vector3(1.5f, 0, 0)));
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
        if (aniC != null)
        {
            StopCoroutine(aniC);
        }
        SoundManager.sound.Play("Block");
        effectC = StartCoroutine(ShowEffect(12, 15, new Vector3(1.5f, 0, 0)));
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
    IEnumerator LongAttackAction() // Defence 관련 세팅
    {
        if (aniC != null)
        {
            StopCoroutine(aniC);
        }
        effectC = StartCoroutine(ShowEffect(16, 22, new Vector3(0, 0, 0)));
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

    void GameOver() // 게임오버시 실행되는 함수, 관련된 내용은 여기 안에다가 작업하면 된다
    {
        if (isGameOver) return;
        if (actionC != null)
        {
            StopCoroutine(actionC);
        }
        if (actionA != null)
        {
            StopCoroutine(actionA);
        }
        DataManager.data.saveData.gameData.player.transform.GetChild(0).gameObject.SetActive(false);
        isGameOver = true;
        playerAction = null;
        playerMove.GameOver();
        //Time.timeScale = 0f;
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

    public IEnumerator CallAni(string Name, float time)
    {
        animaiton.SetAnimation(Name);
        yield return new WaitForSeconds(time);
        if(Name != "Down")
        {
            animaiton.SetAnimation("Run");
        }
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
