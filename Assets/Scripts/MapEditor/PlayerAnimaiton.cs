using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAnimaiton : MonoBehaviour
{
    DataManager Data;
    Animator playerAni;

    [SerializeField] public string crruentAni = "";

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

    void Perry()
    {
        if (crruentAni != "Perry")
        {
            crruentAni = "Perry";
            playerAni.SetTrigger("Perry");
        }
    }

    void Down()
    {
        if (crruentAni != "Down")
        {
            crruentAni = "Down";
            playerAni.SetTrigger("Down");
        }
    }

    void Jump()
    {
        if (crruentAni != "Jump")
        {
            crruentAni = "Jump";
            playerAni.SetTrigger("Jump");
        }
    }

    void Landing()
    {
        if (crruentAni != "Landing")
        {
            crruentAni = "Landing";
            playerAni.SetTrigger("Landing");
        }
    }

    void AttackLeft()
    {
        if (crruentAni != "AttackLeft")
        {
            crruentAni = "AttackLeft";
            playerAni.SetTrigger("AttackLeft");
        }
    }

    void AttackRight()
    {
        if (crruentAni != "AttackRight")
        {
            crruentAni = "AttackRight";
            playerAni.SetTrigger("AttackRight");
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

    void DragAttack()
    {
        if (crruentAni != "DragAttack")
        {
            crruentAni = "DragAttack";
            playerAni.SetTrigger("DragAttack");
        }
    }
}
