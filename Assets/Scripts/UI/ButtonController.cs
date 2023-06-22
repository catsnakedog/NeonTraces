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

    DataManager Data;

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
        Data = DataManager.data; //static data

        OptionCanvas = soundmanager.optionCanvas;
        OptionCanvas.SetActive(false); //게임 시작시 메뉴 패널 비활성화
    }

    #region SettingButton
    public void SettingOpen()
    {
        if (GameObject.Find("SettingButton").transform.localScale.x == 1.2f) //두번째 클릭이면
        {
            soundmanager.Play("Test_ClickSFX");
            Invoke("SettingIn", 0.5f);
            GameObject.Find("SettingButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            GameObject.Find("SettingButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
        }
    }
    public void SettingIn() //세팅 메뉴 열기
    {
       OptionCanvas.SetActive(true);
    }
    #endregion

    #region ExitButton
    public void GameExit() //게임 종료
    {

        if (GameObject.Find("ExitButton").transform.localScale.x == 1.2f) //두번째 클릭이면
        {
#if UNITY_EDITOR //유니티 에디터
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //웹
         Application.OpenURL("http://google.com");
#else // pc 및 모바일 앱
        Application.Quit();
#endif
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
        }
    }
    #endregion

    #region StartButton
    public void ClickStartBtn()
    {
        if (GameObject.Find("StartButton").transform.localScale.x == 1.2f) //두번째 클릭이면
        {
            Invoke("StageMenu", 0.5f);
            GameObject.Find("StartButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            GameObject.Find("StartButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
        }
    }
    public void StageMenu() //StartCanvas에서 StageCanvas로
    {
        Debug.Log("StageMenu");
        startMenu.SetActive(false);
        stageMenu.SetActive(true);
    }
    #endregion
    #region BackButton
    public void BackToStartMenu()
    {
        if (GameObject.Find("BackButton").transform.localScale.x == 1.2f) //두번째 클릭이면
        {
            GameObject.Find("BackButton").transform.localScale = new Vector2(1f, 1f);
            stageMenu.SetActive(false);
            startMenu.SetActive(true);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            GameObject.Find("BackButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
        }
    }
    #endregion


    public void LoadGame() //인게임으로
    {
        int num = Data.saveData.gameData.stage;
        string btn = "Stage" + (num+1).ToString();
        
        if (GameObject.Find(btn).transform.localScale.x == 1.2f) //두번째 클릭이면
        {
            GameObject.Find(btn).transform.localScale = new Vector2(1f, 1f);
            SceneManager.LoadScene(SceneToLoadGame);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            GameObject.Find(btn).transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
        }
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

