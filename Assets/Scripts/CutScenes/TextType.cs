using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;
public class TextType : MonoBehaviour
{
    string originText;
    public Text myText;
    
    void Start()
    {
        originText = myText.text;
        myText.text = "";
        StartCoroutine(TypingRoutine());
    }

    IEnumerator TypingRoutine()
    {
        int typingLength = originText.GetTypingLength();
        for (int index = 0; index <= typingLength; index++)
        {
            myText.text = originText.Typing(index);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
