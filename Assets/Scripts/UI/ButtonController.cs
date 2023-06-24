using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    GameObject OptionCanvas;
    public GameObject startMenu;
    public GameObject stageMenu;
    public string SceneToLoadGame; //�ΰ��� ��
    public string SceneToLoadCutScene; //�� ��
    
    SoundManager soundmanager; //�׽�Ʈ
    CanvasFadeEffect CanvasEffect; //�ε� ��ŵ

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
        OptionCanvas.SetActive(false); //���� ���۽� �޴� �г� ��Ȱ��ȭ
    }

    #region SettingButton
    public void SettingOpen()
    {
        soundmanager.Play("Setting");
        Invoke("SettingIn", 0.5f);
    }
    public void SettingIn() //���� �޴� ����
    {
       OptionCanvas.SetActive(true);
    }
    #endregion

    #region ExitButton
    public void ClickExitBtn() //���� ����
    {
        if (GameObject.Find("ExitButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            soundmanager.Play("Door_exit");
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1f, 1f);
            Invoke("GameExit", 0.7f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            soundmanager.Play("Select");
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    public void GameExit()
    {
#if UNITY_EDITOR //����Ƽ ������
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //��
         Application.OpenURL("http://google.com");
#else // pc �� ����� ��
        Application.Quit();
#endif
    }
    #endregion

    #region StartButton
    public void ClickStartBtn()
    {
        if (GameObject.Find("StartButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            soundmanager.Play("Door_play");
            GameObject.Find("StartButton").transform.localScale = new Vector2(1f, 1f);
            Invoke("StageMenu", 0.8f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            soundmanager.Play("Select");
            GameObject.Find("StartButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    public void StageMenu() //StartCanvas���� StageCanvas��
    {
        Debug.Log("StageMenu");
        startMenu.SetActive(false);
        stageMenu.SetActive(true);
    }
    #endregion
    #region BackButton
    public void ClickBackBtn()
    {
        if (GameObject.Find("BackButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            soundmanager.Play("Execute");
            GameObject.Find("BackButton").transform.localScale = new Vector2(1f, 1f);
            Invoke("BackToStartMenu", 1f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            soundmanager.Play("Select");
            GameObject.Find("BackButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    public void BackToStartMenu()
    {
        stageMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    #endregion

    #region StageButton
    public void ClickStageBtn() //�ΰ�������
    {
        int num = Data.saveData.gameData.stage;
        string btn = "Stage" + (num+1).ToString();
        
        if (GameObject.Find(btn).transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            soundmanager.Play("Execute");
            GameObject.Find(btn).transform.localScale = new Vector2(1f, 1f);
            Invoke("LoadGame", 1f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            soundmanager.Play("Select");
            GameObject.Find(btn).transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(SceneToLoadGame);
    }

    public void LoadCutScene() //�ƾ�����
    {
        SceneManager.LoadScene(SceneToLoadCutScene);
    }
    #endregion
    public void canvasSkip()
    {
        CanvasEffect.FadeoutSkip();
    }

}

