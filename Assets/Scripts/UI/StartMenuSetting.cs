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
        gameObject.SetActive(true);  //���� ���۽� ���� �޴� Ȱ��ȭ
        GameObject.Find("StageCanvas").SetActive(false); //���� ���۽� �������� �޴� ��Ȱ��ȭ
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
