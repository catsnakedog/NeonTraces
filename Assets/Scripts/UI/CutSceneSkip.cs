using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneSkip : MonoBehaviour
{
    [SerializeField] Button skipBtn;
    CutSceneStartEnd cutSceneStartEnd;

    void Start()
    {
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameObject.GetComponent<Canvas>().sortingLayerName = "UI";
        gameObject.GetComponent<Canvas>().sortingOrder = 10;
        cutSceneStartEnd = GameObject.Find("Director").GetComponent<CutSceneStartEnd>();
        skipBtn.onClick.AddListener(Skip);
    }

    void Skip()
    {
        Debug.Log("a");
        cutSceneStartEnd.endScene();
    }
}
