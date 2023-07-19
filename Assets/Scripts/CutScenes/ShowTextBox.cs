using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTextBox : MonoBehaviour
{
    public GameObject TalkingObject; //World �� ���ϴ� ������Ʈ
    RectTransform TextBoxRT; // Canvas ���� �����ϴ� TextBox RT

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
