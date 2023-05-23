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
    [SerializeField] public int type;
    [SerializeField] public int index;
    [SerializeField] public int startDot;
    [SerializeField] public int currentDot;
    [SerializeField] public int endDot;
    [SerializeField] public float speed;
    [SerializeField] public float power; //0�̸� ����Ʈ ������ �̵�
    [SerializeField] public float DefaultSpeed;
    [SerializeField] public float DefaultPower;
    [SerializeField] public string className;
    [SerializeField] public List<int> pattern; // 0 ����, 1 ���, 2...(Ư���׼�)

    [SerializeField] public Vector3 defaultV3;

    int cnt;

    float bloodAngleMax;
    float bloodAngleMin;

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

    void Start()
    {
        Data = DataManager.data;
        player = GameObject.Find("Player");
        map = Data.saveData.gameData.map.transform.GetChild(3).GetChild(0).GetChild(0).gameObject;
        playerAction = GameObject.Find("InGameManager").GetComponent<PlayerAction>();
        playerMove = GameObject.Find("InGameManager").GetComponent<PlayerMove>();
        cameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        blood = Data.saveData.gameData.blood;
        bloodBoom = Data.saveData.gameData.bloodBoom;
        bloodAngleMax = 30;
        bloodAngleMin = -30;
        cnt = 0;
        defaultV3 = gameObject.transform.position;
    }

    void Update()
    {
        enemyAction?.Invoke();   
    }

    private void OnTriggerStay2D(Collider2D collision) // player�� �浹�� ����
    {
        if(isActive)
        {
            if (collision.name == "Life") // �÷��̾� ��ü�� �浹�� ���
            {
                playerAction.Death();
            }
            if (collision.name == "Action") // ����, ��� ������ �浹�� ������ ��
            {
                if (playerAction.isAction)
                {
                    EnemyPattern();
                }
            }
        }
    }

    void EnemyPattern() // ����, ��� ���� ���� Ȯ��
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
            Debug.Log("���׾� ����");
        }
    }

    void Attack() // enemy�� ���� �����϶�
    {
        if(playerAction.isDefence)
        {
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
            PlayerRebound(); // player�� �з�����
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void Defence() // enemy�� ��� �����϶�
    {
        if (playerAction.isAttack)
        {
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
            BloodSetting();
            EnemyRebound(); // enemy�� �з�����
            PatternClear();
        }
        else
        {
            playerAction.Death();
        }
    }

    void PatternClear() // ���������� ������ ���� ������ player �ൿ ��Ÿ�� �ʱ�ȭ
    {
        StartCoroutine(HitEffect());
        isActive = false;
        playerAction.isDelay = false;
        playerAction.isAction = false;
        playerAction.isDefence = false;
        playerAction.isAttack = false;
        playerAction.StopCoroutine(playerAction.actionC); // player�� ���� + ��Ÿ�� �ʱ�ȭ
        //playerAction.animaiton.SetAnimation("Run");
        if (playerAction.actionA != null)
        {
            playerAction.StopCoroutine(playerAction.actionA);
        }
        if (cnt == pattern.Count - 1) 
        {
            Death(); // ������ �������� ���
        }
        else
        {
            isActive = true;
            cnt++; // ������ ���� �������� ���� ���� ����
        }
    }
    void Death() // ������ ���� �Ҹ�� ���
    {
        enemyAction = null;
    }

    public void EnemyMoveStart() // startDot -> ���� �����̴� ���� �ݴ���⿡�� ���� ����� ��, endPoint -> ���� � ������ �����̴���
    {
        if(power == 0) // ���� Ư�� �������� �̵��ϴ� ����Ʈ, power 0���� �������ָ� �ȴ�
        {
            if (startDot == endDot)
            {
                Debug.Log("���̵� �Ϸ�");
            }
            else if (startDot > endDot)
            {
                currentDot = startDot - 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", false, false);
            }
            else
            {
                currentDot = startDot + 1;
                endPoint = Data.saveData.mapData[Data.saveData.gameData.stage].moveDots[currentDot].v3;
                MoveAtoB("EnemyMoveStart", true, false);
            }
        }
        else // ���� �ܼ��� �̵��ϴ°� �ƴ϶� �÷��̾�� ���ݴ��ϰ� �и��� ����
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

    void MoveAtoB(string className, bool isCountUp, bool isBlood) // ���� �̵� ��Ű�� ���� �Լ�
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

    void MoveBySpeed() // enemy�� endPoint�� �̵���Ŵ
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, Time.deltaTime * speed);
    }

    void IsEndPoint() // endPoint�� �����Ͽ��°��� �Ǻ�
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
            Data.saveData.gameData.enemyInfo[index].x = gameObject.transform.position.x;
        }
    }

    void PlayerRebound() // player���� �ݵ��� ��
    {
        playerMove.power = DefaultPower;
        playerMove.playerActionSpeed = DefaultSpeed;
        playerMove.BackOrFront = true;
        playerMove.Rebound();
    }

    void EnemyRebound() // enemy���� �ݵ��� ��
    {
        power = DefaultPower;
        speed = DefaultSpeed;
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

    ~EnemySetting() // �Ҹ���
    {
        enemyAction = null;
    }
}
