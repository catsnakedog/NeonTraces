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
    Sound_Manager sm; //테스트

    public GameObject loading;
    private CanvasGroup canvasGroup;
    public float fadeTime;

    private void Start()
    {
        sm = Sound_Manager.sound;
        loading.SetActive(true);    //로딩 화면 활성화
        startMenu.SetActive(true);  //게임 시작시 시작 메뉴 활성화
        stageMenu.SetActive(false); //게임 시작시 스테이지 메뉴 비활성화
        menuPanel.SetActive(false); //게임 시작시 메뉴 패널 비활성화
        canvasGroup = loading.GetComponent<CanvasGroup>();
    }

    public void SettingOpen()
    {
        sm.Play("Test_ClickSFX");
        Invoke("SettingIn", 0.5f);
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
#if UNITY_EDITOR //유니티 에디터
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //웹
         Application.OpenURL("http://google.com");
#else // pc 및 모바일 앱
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
    public void canvasSkip()
    {
        
        StartCoroutine("FadeOut");
        if (canvasGroup.alpha < 0.1f)
        {
            StopCoroutine("FadeOut");
            loading.SetActive(false);
        }
    }
    public IEnumerator FadeOut()
    {
        //yield return new WaitForSeconds(3.0f);
        float accumTime = 0f;
        while (accumTime < fadeTime)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, accumTime / fadeTime);
            yield return 0;
            accumTime += Time.deltaTime;
        }
        canvasGroup.alpha = 0f;
    }
}

