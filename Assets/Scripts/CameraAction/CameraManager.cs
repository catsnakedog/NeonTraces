using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Shake shake;
    DataManager Data;

    void Start()
    {
        Data = DataManager.data;
    }
    public void CameraAction(string actionName)
    {
        StartCoroutine(actionName);
    }

    IEnumerator ShakeAction()
    {
        shake.Shakecamera();
        yield return new WaitForSeconds(0f);
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
