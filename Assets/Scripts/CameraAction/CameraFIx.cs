using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    DataManager Data;
    public float camsize = 10;
    public GameObject mainCamera;
    public Camera mainCameraC;
    public GameObject target;
    public float camSpeed;
    public bool isShake = false;
    private bool secondframe = false;
    private int framecount = 0;
    public float ShakePower = 1f;

    public float xCorrection;
    public float yCorrection;
    void Start()
    {
        Data = DataManager.data;
        camSpeed = 0.5f;
        mainCamera = GameObject.Find("MainCamera");
        target = GameObject.Find("Player");
        mainCameraC = mainCamera.GetComponent<Camera>();
    }
    void FixedUpdate()
    {
        camsize = Data.saveData.gameData.camsize;
        mainCameraC.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camSpeed);
        if (isShake == true)
        {
            Vector3 startPosition = mainCamera.transform.position;
            if (framecount < 30)
            {
                framecount+=1;
                ShakePower -= 0.04f;
                if (secondframe == false)   
                {
                    mainCamera.transform.position = new Vector3(target.transform.position.x+xCorrection, target.transform.position.y+yCorrection, mainCamera.transform.position.z);
                    secondframe = true;
                }
                else
                {
                    mainCamera.transform.position = startPosition + Random.onUnitSphere * ShakePower;
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                        mainCamera.transform.position.y, -10f);
                    secondframe = false;
                }
            }
            else
            {
                isShake = false;
                framecount = 0;
                ShakePower = 1f;
            }
        }
        else
        {
            mainCamera.transform.position = new Vector3(target.transform.position.x+xCorrection, target.transform.position.y+yCorrection, mainCamera.transform.position.z);
        }
    }
}
// 전역변수 camsize 이용해서 매 업데이트 마다 해당 사이즈로 변경
// 더 빠른 확대, 축소 이펙트 원할 시 camspeed도 함께 조정