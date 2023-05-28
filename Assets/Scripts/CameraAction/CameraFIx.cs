using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class CameraFix : MonoBehaviour
{
    DataManager Data;
    BGManager BGManager;
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
        ShakePower = 1f;
        mainCamera = GameObject.Find("MainCamera");
        target = GameObject.Find("Player");
        BGManager = GameObject.Find("InGameManager").GetComponent<BGManager>();
        mainCameraC = mainCamera.GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        camsize = Data.saveData.gameData.camsize;
        mainCameraC.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, camsize, camSpeed);
        if (BGManager.BGObject != null)
        {
            BGManager.BGObject.transform.localScale = new Vector3(Data.saveData.gameData.camsize / 10f,
                Data.saveData.gameData.camsize / 10f, 1);
        }

        if (isShake == true)
        {
            framecount = 30;
            isShake = false;
            ShakePower = 1f;
        }

        if (framecount > 0)
        {
            Vector3 startPosition = mainCamera.transform.position;
            if (secondframe == false&& framecount % 2==0)
            {
                mainCamera.transform.position = new Vector3(target.transform.position.x+xCorrection,
                    target.transform.position.y + ShakePower+yCorrection, -10f);
                secondframe = true;
                ShakePower *= 0.7f;
            }
            else if (secondframe == true && framecount%2 == 0)
            {
                mainCamera.transform.position = new Vector3(target.transform.position.x+xCorrection,
                    target.transform.position.y - ShakePower+yCorrection, -10f);
                secondframe = false;
                ShakePower *= 0.7f;
            }
            framecount -= 1;
        }
        else
        {
            mainCamera.transform.position = new Vector3(target.transform.position.x + xCorrection,
                target.transform.position.y + yCorrection, mainCamera.transform.position.z);
        }
    }
}
// 전역변수 camsize 이용해서 매 업데이트 마다 해당 사이즈로 변경
// 더 빠른 확대, 축소 이펙트 원할 시 camspeed도 함께 조정