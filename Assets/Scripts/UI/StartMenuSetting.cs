using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSetting : MonoBehaviour
{
    SoundManager soundmanager;
    private CanvasGroup canvasGroup;
    bool once = true;

    float time = 0.0f;

    void Start()
    {
        soundmanager = SoundManager.sound;
        DataManager.data.saveData.gameData.crruentScene = "UI";
        canvasGroup = GameObject.Find("LoadingCanvas").GetComponent<CanvasGroup>();
        gameObject.SetActive(true);  //게임 시작시 시작 메뉴 활성화
        GameObject.Find("StageCanvas").SetActive(false); //게임 시작시 스테이지 메뉴 비활성화
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha < 0.4f) // 로고 꺼지면
        {
            if (once) // 메인 환경음 시작
            {
                soundmanager.Play("mainscreen");
                once = false;
            }

            //10초에 한번씩 해당 SFX 재생
            if (time > 10.0f)
            {
                time = 0.0f;
                soundmanager.Play("mainscreen_screen_flashing");
            }
            else
            {
                time += Time.deltaTime;
            }
        }

    }
}
