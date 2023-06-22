using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameLogoMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float scaleSpeed = 1f;
    private GameObject target;
    private RectTransform targetRT;
    private RectTransform RT;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Target");
        targetRT = target.GetComponent<RectTransform>();
        RT = GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GetComponent<Image>().color.a == 1)
        {
            time += Time.deltaTime; //�̵��� �ð�
            MovePath();
            if (0.1f * scaleSpeed * time < RT.localScale.x) //��ǥ ũ��� ������ �� ���� ũ�� ����)
            {
                Scale();
            }
        }
    }


    public void MovePath()
    {
        RT.localPosition = Vector3.MoveTowards
            (RT.localPosition, targetRT.localPosition, 100f * moveSpeed * Time.deltaTime); //��ǥ ��ġ���� �ش� speed�� �̵�
    }
    public void Scale()
    {
        RT.localScale = new Vector3(RT.localScale.x - 0.1f * scaleSpeed * Time.deltaTime,
                                        RT.localScale.y - 0.1f * scaleSpeed * Time.deltaTime,
                                        RT.localScale.z - 0.1f * scaleSpeed * Time.deltaTime);
    }

}
