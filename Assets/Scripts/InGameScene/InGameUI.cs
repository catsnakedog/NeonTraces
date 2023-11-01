using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    public void TimeSetting(bool value)
    {
        Time.timeScale = value ? 1f : 0f;
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

    }

    public void GoMain()
    {
        SoundManager.sound.Stop("BG");
        Time.timeScale = 1f;
        SoundManager.sound.BGMPause(false);
        SceneManager.LoadScene("UI");
    }
}
