using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] public bool isAttack;
    [SerializeField] public bool isDefence;
    [SerializeField] public bool isAction;
    [SerializeField] public bool isDelay;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackMotionTime;
    [SerializeField] private float defenceMotionTime;

    public Coroutine actionC;

    public void Attack()
    {
        if (!isDelay) actionC = StartCoroutine("AttackAction");
    }
    public void Defence()
    {
        if (!isDelay) actionC = StartCoroutine("DefenceAction");
    }

    public void Death()
    {
        GameOver();
    }

    IEnumerator AttackAction()
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
    IEnumerator DefenceAction()
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

    void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("게임오버");
    }
}
