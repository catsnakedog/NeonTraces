using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonEvent : MonoBehaviour
{
    //버튼에 적용하는 selectBox에 대한 스크립트
    //selctBox는 반드시 버튼의 첫번째 Child

    public bool bButtonClicked = false; //각 버튼별 클릭여부
    bool pointerEnter = false; // 버튼 위에 포인터 올라가있는지 여부

    private void OnEnable()
    {
        if(transform.GetChild(0).GetComponent<FadeEffect>() != null)
            transform.GetChild(0).GetComponent<FadeEffect>().enabled = false;
        transform.GetChild(0).GetComponent<Image>().color = Color.white;
    }

    private void OnDisable()
    {
        bButtonClicked = false;
        //transform.localScale = new Vector2(1f, 1f);
        //transform.GetChild(0).localScale = new Vector2(1f, 1f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void PointerEnter()
    {
        pointerEnter = true;
        Debug.Log("enter");
        ButtonEvent[] be = GetComponentsInChildren<ButtonEvent>(); //해당 컴포넌트 가지고있는 child 배열 (자신 포함)

        if (be.Length > 1) // 자신 제외 더 있다면
            for (int i = 1; i < be.Length; i++)
            {
                if (be[i].GetComponent<ButtonEvent>().pointerEnter) // 자식 오브젝트에 포인터 들어가있으면
                    return;
            }
        //자식이 아닌 자신에게 포인터 들어가있으면    
        gameObject.transform.GetChild(0).gameObject.SetActive(true); //selectBox 표시

        //transform.GetChild(0).localScale = new Vector2(1.2f, 1.2f); // 모바일 빌드 후 확인 필요
    }
    public void PointerExit()
    {
        pointerEnter = false;
        //transform.localScale = new Vector2(1f, 1f);
        if (!bButtonClicked) //클릭된것이 아니라면
        {
            Debug.Log("exit");
            //transform.GetChild(0).localScale = new Vector2(1f, 1f); //selectBox 크기
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void PointerSelect()
    {
        Debug.Log("select");
        //transform.GetChild(0).localScale = new Vector2(1.2f, 1.2f);
        //버튼 클릭 체크는 controller에서
    }
    public void PointerDeselect()
    {
        Debug.Log("DEselect");
        bButtonClicked = false;
        //transform.localScale = new Vector2(1f, 1f);
        //transform.GetChild(0).localScale = new Vector2(1f, 1f);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SelectBoxBlink(bool onOff)
    {
        //5번 깜빡임
        //for (int i = 0; i < 5; i++)
        //{
        //    transform.GetChild(0).GetComponent<Image>().DOFade(0.0f, time / 5);
        //}
        //transform.GetChild(0).GetComponent<Image>().color = Color.white;
        transform.GetChild(0).GetComponent<FadeEffect>().enabled = onOff;
    }
}
