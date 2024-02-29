using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public void TimeSetting(bool value)
    {
        if (value)
            if (GameObject.Find("TutoPopUpOn"))
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
        if(value)
            SoundManager.sound.BGMPause(false);
        else
            SoundManager.sound.BGMPause(true);
    }

    public void ReStart()
    {
        SoundManager.sound.Stop("BG");
        Time.timeScale = 1f;
        SoundManager.sound.BGMPause(false);
        SceneManager.LoadScene("InGameScene");
    }

    public void Setting()
    {
        SoundManager.sound.OpenOption(); //게임 시작시 메뉴 패널 비활성화
    }

    public void GoMain()
    {
        SoundManager.sound.Stop("BG");
        Time.timeScale = 1f;
        SoundManager.sound.BGMPause(false);
        SceneManager.LoadScene("UI");
    }
}
