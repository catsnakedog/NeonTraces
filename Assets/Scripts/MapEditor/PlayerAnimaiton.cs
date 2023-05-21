using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimaiton : MonoBehaviour
{
    DataManager Data;
    Animator playerAni;

    string crruentAni;

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
        if (crruentAni != "Idle")
        {
            crruentAni = "Idle";
            playerAni.SetTrigger("Idle");
        }
    }

    void Run()
    {
        if(crruentAni != "Run")
        {
            crruentAni = "Run";
            playerAni.SetTrigger("Run");
        }
    }

    void Attack()
    {
        if (crruentAni != "Attack")
        {
            crruentAni = "Attack";
            playerAni.SetTrigger("Attack");
        }
    }

    void Defence()
    {
        if (crruentAni != "Defence")
        {
            crruentAni = "Defence";
            playerAni.SetTrigger("Defence");
        }
    }

    void Drag()
    {
        if(crruentAni != "Drag")
        {
            crruentAni = "Drag";
            playerAni.SetTrigger("Drag");
        }
    }
}
