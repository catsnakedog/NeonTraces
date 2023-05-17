using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
    public bool isShake = false;
    private bool secondframe = false;
    private int framecount = 0;
    void Start()
    {
        Data = DataManager.data;
        camSpeed = 0.5f;
        mainCamera = GameObject.Find("MainCamera");
        target = GameObject.Find("Player");
        mainCameraC = mainCamera.GetComponent<Camera>();
    }
    void LateUpdate()
    {
        camsize = Data.saveData.gameData.camsize;
        mainCameraC.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camSpeed);
        if (isShake == true)
        {
            Vector3 startPosition = mainCamera.transform.position;
            if (framecount < 120)
            {
                framecount+=1;
                if (secondframe == false)
                {
                    mainCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCamera.transform.position.z);
                    secondframe = true;
                }
                else
                {
                    mainCamera.transform.position = startPosition + Random.insideUnitSphere * 0.8f;
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                        mainCamera.transform.position.y, -10f);
                    secondframe = false;
                }
            }
            else
            {
                isShake = false;
                framecount = 0;
            }
        }
        else
        {
            mainCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
// 전역변수 camsize 이용해서 매 업데이트 마다 해당 사이즈로 변경
// 더 빠른 확대, 축소 이펙트 원할 시 camspeed도 함께 조정