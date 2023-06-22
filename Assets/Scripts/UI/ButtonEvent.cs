using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{

    public void PointerEnter()
    {
        //transform.localScale = new Vector2(1.2f, 1.2f); // 모바일 빌드 후 확인 필요
    }
    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

}
