using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    DataManager Data;
    public float camsize = 5;
    public GameObject mainCamera;
    public Camera mainCameraC;
    public GameObject target;
    public float camSpeed;
    void Start()
    {
        Data = DataManager.data;
        camSpeed = 1f;
        mainCamera = GameObject.Find("MainCamera");
        target = GameObject.Find("Player");
        mainCameraC = mainCamera.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        camsize = Data.saveData.gameData.camsize;
        mainCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCamera.transform.position.z);
        mainCameraC.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camSpeed);
    }
}
// �������� camsize �̿��ؼ� �� ������Ʈ ���� �ش� ������� ����
// �� ���� Ȯ��, ��� ����Ʈ ���� �� camspeed�� �Բ� ����