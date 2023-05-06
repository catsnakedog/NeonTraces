using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence; // �÷��̾ ���� or ��� ������ Ȯ���ϴ� ������
    [SerializeField] public bool isAction; // EnemySetting�� ��ȣ�ۿ��Ҷ� ����, ��� ������ �Ǻ��ϴ� ����
    [SerializeField] public bool isDelay; // ������ Ȯ�ο� ����
    [SerializeField] private float attackDelay; // ����ݽ� ������
    [SerializeField] private float attackMotionTime; // ���� ���ϸ��̼� ����ð�
    [SerializeField] private float defenceMotionTime; // ��� ���ϸ��̼� ����ð�

    public Coroutine actionC;

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
        isAttack = true;
        isAction = true;
        isDelay = true;
        yield return new WaitForSeconds(attackMotionTime);
        isAction = false;
        isAttack = false;
        yield return new WaitForSeconds(attackDelay);
        isDelay = false;
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
