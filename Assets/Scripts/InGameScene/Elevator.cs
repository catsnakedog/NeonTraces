using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    GameObject _ch;
    Camera _mainCamera;

    void Start()
    {
        _mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().worldCamera = _mainCamera;
        gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
        gameObject.GetComponent<Canvas>().sortingOrder = 9;
        _ch = transform.Find("Ch").gameObject;
    }

    void FixedUpdate()
    {
        _ch.transform.localPosition = new Vector3(-12, -145, -4800);
    }
}
