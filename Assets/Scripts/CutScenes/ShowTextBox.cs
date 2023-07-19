using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextBox : MonoBehaviour
{
    public GameObject TalkingObject; //World 상 말하는 오브젝트
    RectTransform TextBoxRT; // Canvas 내에 존재하는 TextBox RT

    TextType typing;


    // Start is called before the first frame update
    void Start()
    {
        //RectTransform CanvasRect = Canvas.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var screenToWorldPosition = Camera.main.ScreenToWorldPoint(TextBoxRT.transform.position);

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, TextBoxRT.transform.position);
    }
}
