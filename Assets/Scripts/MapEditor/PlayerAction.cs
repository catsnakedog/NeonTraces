using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    DataManager Data;

    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // �÷��̾ ���� or ��� ������ Ȯ���ϴ� ������
    [SerializeField] public bool isAction; // EnemySetting�� ��ȣ�ۿ��Ҷ� ����, ��� ������ �Ǻ��ϴ� ����
    [SerializeField] public bool isDelay; // ������ Ȯ�ο� ����
    [SerializeField] private float attackDelay; // ����ݽ� ������
    [SerializeField] private float attackMotionTime; // ���� ���ϸ��̼� ����ð�
    [SerializeField] private float defenceMotionTime; // ��� ���ϸ��̼� ����ð�

    public Coroutine actionC;
    public Coroutine actionA;

    AfterImage afterImage;

    void Start()
    {
        Data = DataManager.data;
        afterImage = transform.GetComponent<AfterImage>();
    }

    public void Attack() // �÷��̾� ����
    {
        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence() // �÷��̾� ���
    {
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }

    public void Death() // �÷��̾ ������ ����
    {
        GameOver();
    }

    IEnumerator AttackAction() // Attack ���� ����
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
    IEnumerator DefenceAction() // Defence ���� ����
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

    void GameOver() // ���ӿ����� ����Ǵ� �Լ�, ���õ� ������ ���� �ȿ��ٰ� �۾��ϸ� �ȴ�
    {
        Time.timeScale = 0f;
        Debug.Log("���ӿ���");
    }
}
