using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject startMenu;
    public GameObject stageMenu;
    public string SceneToLoadGame; //인게임 씬
    public string SceneToLoadCutScene; //컷 씬

    private void Start()
    {
        startMenu.SetActive(true);  //게임 시작시 시작 메뉴 활성화
        stageMenu.SetActive(false); //게임 시작시 스테이지 메뉴 비활성화
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

    public void BackToStartMenu()
    {
        stageMenu.SetActive(false);
        startMenu.SetActive(true);
    }

    public void LoadGame() //인게임으로
    {
        SceneManager.LoadScene(SceneToLoadGame);
    }

    public void LoadCutScene() //컷씬으로
    {
        SceneManager.LoadScene(SceneToLoadCutScene);
    }
}

