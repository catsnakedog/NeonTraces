using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLogoMove : MonoBehaviour
{
    //로고 이동 시간
    [SerializeField] float moveAndScaleTime = 2.0f;

    private RectTransform targetRT;
    private RectTransform RT;

    float distance;
    Vector3 scaleVector;

    // Start is called before the first frame update
    void Start()
    {
        targetRT = GameObject.Find("Target").GetComponent<RectTransform>();
        RT = GetComponent<RectTransform>();

        distance = Vector3.Distance(RT.localPosition, targetRT.localPosition);
        scaleVector = RT.localScale - targetRT.localScale;
    }

    // Update is called once per frame
    public void Update()
    {
        if (GetComponent<Image>().color.a == 1) //로고 켜지면 이동
        {
            MovePath();

            if (Vector3.Magnitude(RT.localScale - targetRT.localScale) > 0.01f) //목표 크기와 동일할 때 까지 크기 감소)
            {
                Scale();
            }
        }
    }


    public void MovePath()
    {
        RT.localPosition = Vector3.MoveTowards
            (RT.localPosition, targetRT.localPosition, distance / moveAndScaleTime * 0.01f); //목표 위치까지 해당 속도 로 이동

    }
    public void Scale()
    {
        RT.localScale = new Vector3(RT.localScale.x - scaleVector.x / moveAndScaleTime * 0.01f,
                                        RT.localScale.y - scaleVector.y / moveAndScaleTime * 0.01f,
                                        RT.localScale.z - scaleVector.y / moveAndScaleTime * 0.01f);
    }

}
