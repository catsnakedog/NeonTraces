using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Shake shake;

    void Start()
    {
            
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
}
