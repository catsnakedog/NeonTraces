using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Camerafix : MonoBehaviour
{
    public float camsize = 5;
    public GameObject A;
    Transform AT;
    public static float camspeed;
    void Start()
    {
        AT = A.transform;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(AT.position.x, AT.position.y, transform.position.z);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camspeed);
    }
}
// 전역변수 camsize 이용해서 매 업데이트 마다 해당 사이즈로 변경
// 더 빠른 확대, 축소 이펙트 원할 시 camspeed도 함께 조정