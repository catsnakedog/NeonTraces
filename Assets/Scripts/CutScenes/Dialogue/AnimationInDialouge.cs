using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnimTiming
{
    AfterContext,
    DuringContext,
}

[System.Serializable]
public struct Timing // 무슨 타이밍에 어떤 대사에서 무슨 애니메이션을 할지
{
    public AnimTiming AnimationTiming;

    public string context;
    public string animationName;
    
}

[System.Serializable]
public struct WhatText // 어떤 텍스트에서 타이밍 체크할지
{
    public Text text;
    public Timing[] timing;
}



public class AnimationInDialouge : MonoBehaviour
{
    [SerializeField] WhatText[] whatText; // 누구 텍스트의 대사에서 애니메이션 할지

    Animator animator;
    static public bool AnimEnd = true;

    //public AnimTiming animTiming; // 어떤 타이밍할지 드롭다운으로 선택

    void Start()
    {
        if(GetComponent<Animator>() != null)
            animator = this.GetComponent<Animator>();
    }


    Timing AtTiming;
    public void TimingCheck()
    {
        for (int i = 0; i < whatText.Length; i++)
        {
            for (int j = 0; j < whatText[i].timing.Length; j++)
            {
                AtTiming = whatText[i].timing[j];
                //if (AtTiming.AnimationTiming == AnimTiming.AfterContext)
                //{
                //    animator.Play(whatText[i].timing[j].animationName);
                //}
                //else if(whatText[i].timing[j].AnimationTiming == AnimTiming.DuringContext)
                //{

                //}

                switch (AtTiming.AnimationTiming)
                {
                    case AnimTiming.AfterContext:
                        if (whatText[i].text.text == AtTiming.context)
                        {
                            animator.Play(whatText[i].timing[j].animationName);
                            
                            AnimEnd = false; //애니메이션 끝나야 넘어갈수있음
                        }
                        break;

                    case AnimTiming.DuringContext:
                        break;
                }
            }

        }
    }


    // Update is called once per frame
    void Update()
    {
        TimingCheck();
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            AnimEnd = true;
    }


}
