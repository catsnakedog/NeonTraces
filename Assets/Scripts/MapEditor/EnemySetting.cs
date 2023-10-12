using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using UnityEngine;

public class EnemySetting : MonoBehaviour
{
    [SerializeField] public bool isActive = true;
    [SerializeField] public bool isCountUp;
    [SerializeField] public bool isBlood;
    [SerializeField] public bool isBack;
    [SerializeField] public bool isAttackAni;
    [SerializeField] public int type;
    [SerializeField] public int index;
    [SerializeField] public int startDot;
    [SerializeField] public int currentDot;
    [SerializeField] public int endDot;
    [SerializeField] public float power; //0이면 포인트 끝까지 이동
    [SerializeField] public float DefaultSpeed;
    [SerializeField] public float DefaultPower;
    [SerializeField] public string className;
    [SerializeField] public List<int> pattern; // 0 공격, 1 방어, 2...(특수액션)
    [SerializeField] public float xPoint;
    [SerializeField] public float speed;

    [SerializeField] public Vector3 defaultV3;

    [SerializeField] public float backPower;
    [SerializeField] public float backSpeed;

    [SerializeField] public List<Sprite> Effect; 

    int cnt;

    float bloodAngleMax;
    float bloodAngleMin;

    int moveStartDot;
    int moveEndDot;

    GameObject player;
    GameObject blood;
    GameObject bloodBoom;
    GameObject bloodBoomObject;
    GameObject map;
    DataManager Data;

    Vector3 startPoint;
    Vector3 endPoint;

    Action enemyAction = null;
    PlayerAction playerAction;
    PlayerMove playerMove;
    CameraManager cameraManager;
    Animator enemyAnimator;

    void Start()
    {
        Data = DataManager.data;
        player = GameObject.Find("Player");
        map = Data.saveData.gameData.map.transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
        playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        playerMove = GameObject.Find("InGameManager").GetComponent<PlayerMove>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        enemyAnimator = GetComponent<Animator>();
        blood = Data.saveData.gameData.blood;
        bloodBoom = Data.saveData.gameData.bloodBoom;
        bloodAngleMax = 30;
        bloodAngleMin = -30;
        cnt = 0;
        isBack = false;
        defaultV3 = gameObject.transform.position;
    }

    void Update()
    {
        enemyAction?.Invoke();   
    }

    private void OnTriggerStay2D(Collider2D collision) // player와 충돌시 판정
    {
        if (isBack)
            return;
        if(isActive)
        {
            if (collision.name == "Life") // 플레이어 본체와 충돌시 사망
            {
                playerAction.Death();
            }
            if (collision.name == "Action") // 공격, 방어 범위와 충돌시 판정에 들어감
            {
                if (playerAction.isAction)
                {
                    EnemyPattern();
                }
            }
        }
    }

    void EnemyPattern() // 공격, 방어 성공 판정 확인
    {
        if (pattern[cnt] == 0)
        {
            Attack();
        }
        else if (pattern[cnt] == 1)
        {
            Defence();
        }
        else if (pattern[cnt] == 2)
        {
            StrongAttack();
        }
        else if (pattern[cnt] == 3)
        {
            StrongDefence();
        }
        else
        {
            Debug.Log("버그야 버그");
        }
    }

    void Attack() // enemy가 공격 상태일때
    {
        if(playerAction.isDefence)
        {
            playerAction.aniC = StartCoroutine(playerAction.CallAni("Perry", 0.49f));
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void StrongAttack()
    {
        if (playerAction.isDefence)
        {
            //SoundManager.sound.Play("Main_parry_A2");
            playerAction.aniC = StartCoroutine(playerAction.CallAni("Perry", 0.49f));
            EnemyRebound(); // enemy가 밀려난다
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void Defence() // enemy가 방어 상태일때
    {
        if (playerAction.isAttack)
        {
            SoundManager.sound.Play("main_hit");
            BloodSetting();
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void StrongDefence()
    {
        if (playerAction.isAttack)
        {
            SoundManager.sound.Play("main_hit");
            BloodSetting();
            EnemyRebound(); // enemy가 밀려난다
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void PatternClear() // 성공적으로 판정에 성공 했을때 player 행동 쿨타임 초기화
    {
        StartCoroutine(HitEffect());
        isActive = false;
        playerAction.isDelay = false;
        playerAction.isAction = false;
        playerAction.isDefence = false;
        playerAction.isAttack = false;
        enemyAction = null;
        playerAction.StopCoroutine(playerAction.actionC); // player의 상태 + 쿨타임 초기화
        //playerAction.animaiton.SetAnimation("Run");
        if (playerAction.actionA != null)
        {
            playerAction.StopCoroutine(playerAction.actionA);
        }
        if (cnt == pattern.Count - 1) 
        {
            Death(); // 패턴이 끝났으면 사망
        }
        else
        {
            EnemyMoveStart();
            isActive = true;
            cnt++; // 패턴이 아직 남았으면 다음 패턴 실행
        }
    }
    void Death() // 패턴이 전부 소모시 사망
    {
        enemyAnimator.SetTrigger("IsDie");
        bloodBoomObject = Instantiate(bloodBoom, transform.position, Quaternion.identity);
        bloodBoomObject.transform.SetParent(gameObject.transform);
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            bloodBoomObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        enemyAction = null;
    }

    public void EnemyMoveStart() // startDot -> 적이 움직이는 방향 반대방향에서 가장 가까운 점, endPoint -> 적이 어떤 점까지 움직이는지
    {
        if(power == 0) // 적이 특정 지점까지 이동하는 이펙트, power 0으로 세팅해주면 된다
        {
            if (startDot == endDot)
            {
                enemyAnimator.SetBool("IsRun", false);
                if (isAttackAni) enemyAction += IsAttack;
                Debug.Log("적이동 완료");
            }
            else if (startDot > endDot)
            {
                enemyAnimator.SetBool("IsRun", true);
                if (isAttackAni) enemyAction += IsAttack;
                currentDot = startDot - 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", false, false);
            }
            else
            {
                enemyAnimator.SetBool("IsRun", true);
                if (isAttackAni) enemyAction += IsAttack;
                currentDot = startDot + 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", true, false);
            }
        }
        else // 적이 단순히 이동하는게 아니라 플레이어에게 공격당하고 밀릴때 판정
        {
            startPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[startDot].v3;
            endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[endDot].v3;
            Vector3 vDist = endPoint - startPoint;
            Vector3 vDir = vDist.normalized;
            Vector3 tempEndV = endPoint;
            endPoint = gameObject.transform.position + (vDir * power);
            float a = (gameObject.transform.position - tempEndV).magnitude;
            float b = (gameObject.transform.position - endPoint).magnitude;
            if (b > a)
            {
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[endDot].v3;
                MoveAtoB("", true, true);
            }
            else
            {
                MoveAtoB("", true, true);
            }
        }
    }

    void MoveAtoB(string className, bool isCountUp, bool isBlood) // 적을 이동 시키기 위한 함수
    {
        this.className = className;
        this.isCountUp = isCountUp;
        this.isBlood = isBlood;
        if(isBlood)
        {
            bloodBoomObject = Instantiate(bloodBoom, transform.position, Quaternion.identity);
            bloodBoomObject.transform.SetParent(gameObject.transform);
            if(player.transform.position.x > gameObject.transform.position.x)
            {
                bloodBoomObject.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        enemyAction += MoveBySpeed;
        enemyAction += IsEndPoint;
    }

    void MoveBySpeed() // enemy를 endPoint로 이동시킴
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint에 도달하였는가를 판별
    {
        if (gameObject.transform.position == endPoint)
        {
            enemyAction -= MoveBySpeed;
            enemyAction -= IsEndPoint;
            if(isCountUp)
            {
                startDot++;
            }
            else
            {
                startDot--;
            }
            if(isBlood)
            {
                bloodBoomDestroy();
            }
            if(className != "")
            {
                Invoke(className, 0f);
            }
            if(isBack)
            {
                isBack = false;
            }
            Data.saveData.gameData.enemyInfo[index].x = gameObject.transform.position.x;
        }
    }

    void PlayerRebound() // player에게 반동을 줌
    {
        return;
        playerMove.power =backPower;
        playerMove.playerActionSpeed = backSpeed;
        playerMove.BackOrFront = true;
        playerMove.Rebound();
    }

    void EnemyRebound() // enemy에게 반동을 줌
    {
        isBack = true;
        power = backPower;
        speed = backSpeed;
        startDot = playerMove.crruentMoveDot;
        endDot = playerMove.crruentMoveDot+1;
        EnemyMoveStart();
    }

    void BloodSetting()
    {
        Vector3 dir = transform.position - player.transform.position;
        Quaternion rot = Quaternion.LookRotation(dir.normalized);
        float cor = UnityEngine.Random.Range(bloodAngleMin, bloodAngleMax);
        Vector3 temp = new Vector3(transform.position.x+0.75f, transform.position.y, transform.position.z);
        GameObject blood = Instantiate(this.blood, temp, rot);
        blood.transform.eulerAngles = new Vector3(blood.transform.eulerAngles.x, 0f, blood.transform.eulerAngles.z + cor);
        blood.transform.SetParent(map.transform, true);
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            blood.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    IEnumerator bloodBoomDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(bloodBoomObject);
    }

    IEnumerator HitEffect()
    {
        cameraManager.CameraAction("ShakeAction");
        cameraManager.CameraAction("ZoomInAction");
        yield return new WaitForSeconds(0.05f);
        cameraManager.CameraAction("ZoomOutAction");
    }

    float attackAniPos;
    float attackAniTime;

    public void EnemyPosition()
    {
        List<MoveDot> moveDots = DataManager.data.saveData.mapData[DataManager.data.saveData.gameData.stage].moveDots;

        int num = 0;
        int dotPosition1 = 0;
        int dotPosition2 = 0;
        speed = DefaultSpeed;
        enemyAction += IsAttack;

        foreach (MoveDot dot in moveDots)
        {
            if(transform.GetChild(0).position.x <= dot.v3.x)
            {
                dotPosition1 = num - 1;
                break;
            }
            num++;
        }

        num = 0;
        foreach (MoveDot dot in moveDots)
        {
            if (transform.position.x <= dot.v3.x)
            {
                dotPosition2 = num;
                break;
            }
            num++;
        }

        float distance = 0;

        for(int i = (dotPosition1 + 1); i < (dotPosition2 - 1); i++)
        {
            distance += Vector3.Distance(moveDots[i].v3, moveDots[i + 1].v3);
        }

        if(dotPosition1 + 1 == dotPosition2)
        {
            distance += Vector3.Distance(transform.GetChild(0).position, transform.position);
        }
        else
        {
            distance += Vector3.Distance(moveDots[dotPosition2 - 1].v3, transform.position);
            distance += Vector3.Distance(moveDots[dotPosition1 + 1].v3, transform.GetChild(0).position);

        }

        float time = (distance / speed);

        distance = 0;
        float playerTime = 0;

        moveEndDot = dotPosition1;
        moveStartDot = dotPosition2;

        startDot = moveStartDot;
        endDot = moveEndDot;

        for (int i = dotPosition1; i >= 0; i--)
        {
            if(i == dotPosition1)
            {
                distance += Vector3.Distance(moveDots[i].v3, transform.GetChild(0).position);
                playerTime += distance / moveDots[i].speed;
            }
            else
            {
                distance += Vector3.Distance(moveDots[i].v3, moveDots[i + 1].v3);
                playerTime += distance / moveDots[i].speed;
            }

            if (playerTime > time)
            {
                playerTime -= distance / moveDots[i].speed;
                playerTime = time - playerTime;

                distance = playerTime * moveDots[i].speed;

                if(i == dotPosition1)
                {
                    xPoint = moveDots[i].v3.x + ((transform.GetChild(0).position.x - moveDots[i].v3.x) * (1f - (distance / Vector3.Distance(moveDots[i].v3, transform.GetChild(0).position))));
                }
                else
                {
                    xPoint = moveDots[i].v3.x + ((moveDots[i + 1].v3.x - moveDots[i].v3.x) * (1f - (distance / Vector3.Distance(moveDots[i].v3, moveDots[i + 1].v3))));
                }

                enemyAction += IsPlayerCome;
                break;
            }
            else if (playerTime == time)
            {
                xPoint = moveDots[i].v3.x;
                enemyAction += IsPlayerCome;
                break;
            }

        }

        attackAniPos = (xPoint + speed * attackAniTime);
    }

    void IsPlayerCome()
    {
        if(DataManager.data.saveData.gameData.player.transform.position.x >= xPoint)
        {
            EnemyMoveStart();
            enemyAction -= IsPlayerCome;
        }
    }

    void IsAttack()
    {
        if(DataManager.data.saveData.gameData.player.transform.position.x >= attackAniPos)
        {
            enemyAnimator.SetTrigger("IsAttack");
            enemyAction -= IsAttack;
        }
    }
}