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
    public string[] SceneToLoadCutScene; //컷 씬
    
    public SoundManager soundmanager; //테스트
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
    public void ClickSettingBtn() //설정 버튼 클릭 시
    {
        tmp = GameObject.Find("SettingButton");

        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 애니메이션+실행
        {
            Debug.Log("SettingButton Clicked");

            tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", true);
           
            soundmanager.Play("Setting");
            Invoke("OpenSetting", 0.6f); //소리 길이만큼 대기 후 실행
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");

            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
        }
    }
    public void OpenSetting() //세팅 메뉴 열기
    {
        tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", false);
        OptionCanvas.SetActive(true);
    }
    #endregion

    #region ExitButton
    public void ClickExitBtn() //게임종료 버튼 클릭 시
    {
        tmp = GameObject.Find("ExitButton");

        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 클릭효과+애니메이션+실행
        {
            Debug.Log("ExitButton Clicked");

            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true); //클릭 효과
            tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", true); //애니메이션
           
            soundmanager.Play("Door_exit");
            Invoke("GameExit", 0.758f);//소리 길이만큼 대기 후 실행
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");
           
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
        }
    }
    public void GameExit() //게임 종료
    {
        tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", false);
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
    public void ClickStartBtn() //시작 버튼 클릭 시
    {
        tmp = GameObject.Find("StartButton");

        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 클릭효과+애니메이션+실행
        {
            Debug.Log("StartButton Clicked");

            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);
            tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", true);
        
            soundmanager.Play("Door_play");
            Invoke("OpenStageMenu", 0.77f);//소리 길이만큼 대기 후 실행
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
        }
    }
    public void OpenStageMenu() //StartCanvas에서 StageCanvas로
    {
        
        Debug.Log("Open StageMenu");
        startMenu.SetActive(false);
        stageMenu.SetActive(true);
        tmp.GetComponent<Animator>().SetBool("ButtonClickSecond", false);
    }
    #endregion

    #region BackButton
    public void ClickBackBtn()
    {
        tmp = GameObject.Find("BackButton");
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 클릭효과+애니메이션+실행
        {
            Debug.Log("BackButton Clicked");

            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);
            
            soundmanager.Play("Execute");
            Invoke("BackToStartMenu", 1f); //소리 길이만큼 대기 후 실행
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
        }
    }
    public void BackToStartMenu()
    {
        Debug.Log("Back to StartMenu");
        stageMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    #endregion

    #region StageButton
    public void ClickStageBtn() //인게임버튼 클릭 시
    {
        int num = Data.saveData.gameData.stage;
        string btn = "Stage" + num.ToString(); // 스테이지 선택 버튼 이름

        tmp = GameObject.Find(btn);
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 클릭효과+애니메이션+실행
        {
            Debug.Log("InGameButton Clicked");

            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);
            
            soundmanager.Play("Execute");
            Invoke("LoadGame", 1f); //소리 길이만큼 대기 후 실행
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;
        }
    }
    public void LoadGame() //인게임으로 이동, BGM 종료
    {
        tmp.GetComponent<ButtonEvent>().bButtonClicked = false;
        soundmanager.Stop();
        SceneManager.LoadScene(SceneToLoadGame);
    }
    #endregion

    #region CutSceneButton
    public void ClickCutSceneBtn(int num) // 컷씬버튼 클릭 시
    {
        string btn = "cutScenePlayer" + num.ToString();

        tmp = GameObject.Find(btn);
        if (tmp.transform.GetChild(0).gameObject.activeSelf && tmp.GetComponent<ButtonEvent>().bButtonClicked) //두번째 클릭이면 클릭효과+애니메이션+실행
        {
            Debug.Log("CutSceneButton Clicked");

            tmp.GetComponent<ButtonEvent>().SelectBoxBlink(true);

            soundmanager.Play("Execute"); // 소리 확인 필요**************************
            StartCoroutine("LoadCutScene", num);
        }
        else //첫번째 클릭이면 선택 표시
        {
            soundmanager.Play("Select");
            tmp.GetComponent<ButtonEvent>().bButtonClicked = true;//
        }
    }
    IEnumerator LoadCutScene(int num) //컷씬으로 이동, BGM 종료
    {
        tmp.GetComponent<ButtonEvent>().SelectBoxBlink(false);
        yield return new WaitForSeconds(1.0f);
        soundmanager.Stop();
        SceneManager.LoadScene(SceneToLoadCutScene[num], LoadSceneMode.Additive); // ui 씬 실행 중 컷씬 실행
    }
    #endregion
    public void canvasSkip()
    {
        CanvasEffect.FadeoutSkip();
    }

}

