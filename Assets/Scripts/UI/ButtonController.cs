using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject startMenu;
    public GameObject stageMenu;
    
    private void Start()
    {
        menuPanel.SetActive(false); //게임 시작시 메뉴 패널 비활성화
    }


    public void SettingIn() //세팅 메뉴 열기
    {
        menuPanel.SetActive(true);
        
    }
    public void SettingOut() //세팅 메뉴 닫기
    {
        menuPanel.SetActive(false);
    }

    public void GameExit() //게임 종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void StageMenu() //StartCanvas에서 StageCanvas로
    {
        Debug.Log("StageMenu");
        startMenu.SetActive(false);
        stageMenu.SetActive(true);
    }

}
