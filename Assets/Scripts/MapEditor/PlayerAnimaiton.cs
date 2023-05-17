using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimaiton : MonoBehaviour
{
    DataManager Data;
    Animator playerAni;

    void Start()
    {
    }

    public void AniSet()
    {
        Data = DataManager.data;
        playerAni = GameObject.Find("Player").GetComponent<Animator>();
    }

    public void SetAnimation(string AniName)
    {
        Invoke(AniName, 0f);
    }

    void Idle()
    {
        playerAni.SetTrigger("Idle");
    }

    void Run()
    {
        playerAni.SetTrigger("Run");
    }

    void Attack()
    {
        playerAni.SetTrigger("Attack");
    }
}
