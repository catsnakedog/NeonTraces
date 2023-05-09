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
// �������� camsize �̿��ؼ� �� ������Ʈ ���� �ش� ������� ����
// �� ���� Ȯ��, ��� ����Ʈ ���� �� camspeed�� �Բ� ����