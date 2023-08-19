using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    GameObject tmp;

    private void Awake()
    {
        CanvasEffect = FindObjectOfType<CanvasFadeEffect>();
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
    public void ClickSettingBtn()
    {
        tmp = GameObject.Find("SettingButton");

        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면
        {
            soundmanager.Play("Setting");
            //tmp.transform.localScale = new Vector2(1f, 1f);
            Invoke("SettingIn", 0.5f);
        }
        else //첫번째 클릭이면 확대
        {
            soundmanager.Play("Select");
            //tmp.transform.localScale = new Vector2(1.2f, 1.2f);

            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;//
        }
    }
    public void SettingIn() //세팅 메뉴 열기
    {
       OptionCanvas.SetActive(true);
    }
    #endregion

    #region ExitButton
    public void ClickExitBtn() //게임 종료
    {
        tmp = GameObject.Find("ExitButton");
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면
        {
            
            soundmanager.Play("Door_exit");
            //tmp.transform.localScale = new Vector2(1f, 1f);
            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);

            Invoke("GameExit", 0.7f);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            soundmanager.Play("Select");
            //tmp.transform.localScale = new Vector2(1.2f, 1.2f);
           
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;//
        }
    }
    public void GameExit()
    {
#if UNITY_EDITOR //유니티 에디터
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //웹
         Application.OpenURL("http://google.com");
#else // pc 및 모바일 앱
        Application.Quit();
#endif
    }
    #endregion

    #region StartButton
    public void ClickStartBtn()
    {
        tmp = GameObject.Find("StartButton");
        Debug.Log("Two");
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면
        {
            soundmanager.Play("Door_play");
            //tmp.transform.localScale = new Vector2(1f, 1f);
            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);

            Invoke("StageMenu", 0.8f);
        }
        else //첫번째 클릭이면 확대
        {
            soundmanager.Play("Select");
            //tmp.transform.localScale = new Vector2(1.2f, 1.2f);
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
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
    public void ClickBackBtn()
    {
        tmp = GameObject.Find("BackButton");
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면
        {
            soundmanager.Play("Execute");
            //GameObject.Find("BackButton").transform.localScale = new Vector2(1f, 1f);
            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);

            Invoke("BackToStartMenu", 1f);
        }
        else //첫번째 클릭이면 확대
        {
            soundmanager.Play("Select");
            //GameObject.Find("BackButton").transform.localScale = new Vector2(1.2f, 1.2f);
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;//
        }
    }
    public void BackToStartMenu()
    {
        stageMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    #endregion

    #region StageButton
    public void ClickStageBtn() //인게임으로
    {
        int num = Data.saveData.gameData.stage;
        string btn = "Stage" + (num+1).ToString();

        tmp = GameObject.Find(btn);
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면
        {
            soundmanager.Play("Execute");
            //GameObject.Find(btn).transform.localScale = new Vector2(1f, 1f);
            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);

            Invoke("LoadGame", 1f);
        }
        else //첫번째 클릭이면 확대, 선택된 효과
        {
            soundmanager.Play("Select");
            //GameObject.Find(btn).transform.localScale = new Vector2(1.2f, 1.2f);
            //+선택된 효과
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;//
        }
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneToLoadGame);
    }

    public void LoadCutScene() //컷씬으로
    {
        SceneManager.LoadScene(SceneToLoadCutScene);
    }
    #endregion
    public void canvasSkip()
    {
        CanvasEffect.FadeoutSkip();
    }

}

