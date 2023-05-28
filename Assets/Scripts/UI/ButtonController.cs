using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    GameObject OptionCanvas;
    public GameObject startMenu;
    public GameObject stageMenu;
    public string SceneToLoadGame; //인게임 씬
    public string SceneToLoadCutScene; //컷 씬
    
    SoundManager soundmanager; //테스트
    CanvasFadeEffect CanvasEffect; //로딩 스킵

    public GameObject loading;
    private CanvasGroup canvasGroup;



    private void Awake()
    {
        CanvasEffect = FindObjectOfType<CanvasFadeEffect>();
        startMenu.SetActive(true);  //게임 시작시 시작 메뉴 활성화
        stageMenu.SetActive(false); //게임 시작시 스테이지 메뉴 비활성화
        canvasGroup = loading.GetComponent<CanvasGroup>();
    }

    void Start()
    {
        soundmanager = SoundManager.sound;
       
        OptionCanvas = soundmanager.optionCanvas;
        OptionCanvas.SetActive(false); //게임 시작시 메뉴 패널 비활성화
    }

    public void SettingOpen()
    {
        soundmanager.Play("Test_ClickSFX");
        Invoke("SettingIn", 0.5f);
    }
    public void SettingIn() //세팅 메뉴 열기
    {

       OptionCanvas.SetActive(true);
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
        CanvasEffect.FadeoutSkip();
    }
}

