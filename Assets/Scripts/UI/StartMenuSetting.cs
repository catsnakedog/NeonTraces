using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSetting : MonoBehaviour
{
    SoundManager soundmanager;
    private CanvasGroup canvasGroup;
    bool once = true;

    void Start()
    {
        soundmanager = SoundManager.sound;

        canvasGroup = GameObject.Find("LoadingCanvas").GetComponent<CanvasGroup>();
        gameObject.SetActive(true);  //게임 시작시 시작 메뉴 활성화
        GameObject.Find("StageCanvas").SetActive(false); //게임 시작시 스테이지 메뉴 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < 0.4f && once)
        {
            soundmanager.Play("Test_BGM");
            once = false;
        }
    }
}
