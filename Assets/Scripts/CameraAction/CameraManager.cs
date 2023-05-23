using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Shake shake;
    CameraFix cameraFix;
    DataManager Data;
    private Transform mainCamera;
    public bool isShake;
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").transform;
        Data = DataManager.data;
        cameraFix = gameObject.GetComponent<CameraFix>();
        shake = gameObject.GetComponent<Shake>();
    }
    public void CameraAction(string actionName)
    {
        Debug.Log(actionName);
        StartCoroutine(actionName);
    }
    IEnumerator ShakeAction()
    {
        cameraFix.isShake = false;
        cameraFix.isShake = true;
        yield return null;
    }
    IEnumerator ZoomInAction()
    {
        Data.saveData.gameData.camsize += 1;
        yield return null;
    }
    IEnumerator ZoomOutAction()
    {
        Data.saveData.gameData.camsize -= 1;
        yield return null;
    }
}
