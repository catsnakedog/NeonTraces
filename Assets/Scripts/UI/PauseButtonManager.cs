using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonManager : MonoBehaviour
{
    public GameObject panel;

    public SoundManager soundmanager;
    GameObject SettingPanel;

    void Start()
    {
        panel.SetActive(false);

        soundmanager = SoundManager.sound;
        SettingPanel = soundmanager.optionCanvas;
        SettingPanel.SetActive(false);
    }


    public void panel_open() // 시간 정지, 패널 열기
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    public void Quit()
    {
        // + quit 버튼 클릭 시 수행 함수 추가
    }

    public void Resume()
    {
        // + Resume 버튼 클릭 시 수행 함수 추가

        panel.SetActive(false);
        Time.timeScale = 1; //시간 정지 해제
    }

    public void Restart()
    {
        // + Restart 버튼 클릭 시 수행 함수 추가
    }

    public void Setting()
    {
        SettingPanel.SetActive(true);
    }


}
