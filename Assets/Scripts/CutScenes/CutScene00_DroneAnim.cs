using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CutScene00_DroneAnim : MonoBehaviour
{
    public Text[] text;
    SpriteRenderer droneRender;
    Animator drone;
    // Start is called before the first frame update
    void Start()
    {
        droneRender = gameObject.GetComponent<SpriteRenderer>();
        drone = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].text == "이쪽")
                droneRender.flipX = true;
            else if (text[i].text == "농담 정도는 할 줄 압니다.")
                drone.Play("Drone_Smile");
            else if (text[i].text == "…진담입니다.")
                droneRender.flipX = false;
            //else if (text[i].text == "'책임자 ÷ x = 선택' 입니다!!")
                
        }
    }
}
