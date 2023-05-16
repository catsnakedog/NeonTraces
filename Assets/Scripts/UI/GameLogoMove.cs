using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLogoMove : MonoBehaviour
{
   
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
       
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.sizeDelta = new Vector2(300, 100);
        //rectTransform.transform.position = new Vector3(-990, 480, 0); //(gameObject.transform.position, new Vector3(-910,490,0), 0.1f);
        //rectTransform.localPosition = new Vector3(-990, 480, 0);
        
        rectTransform.position = Vector3.MoveTowards(rectTransform.position, new Vector3(-25f, 10f, 0f), 0.5f);
    }
}
