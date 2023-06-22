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

    private void Awake()
    {
        CanvasEffect = FindObjectOfType<CanvasFadeEffect>();
        startMenu.SetActive(true);  //���� ���۽� ���� �޴� Ȱ��ȭ
        stageMenu.SetActive(false); //���� ���۽� �������� �޴� ��Ȱ��ȭ
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
        if (GameObject.Find("SettingButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            soundmanager.Play("Test_ClickSFX");
            Invoke("SettingIn", 0.5f);
            GameObject.Find("SettingButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            GameObject.Find("SettingButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    public void SettingIn() //���� �޴� ����
    {
       OptionCanvas.SetActive(true);
    }
    #endregion

    #region ExitButton
    public void GameExit() //���� ����
    {

        if (GameObject.Find("ExitButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
#if UNITY_EDITOR //����Ƽ ������
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //��
         Application.OpenURL("http://google.com");
#else // pc �� ����� ��
        Application.Quit();
#endif
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            GameObject.Find("ExitButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    #endregion

    #region StartButton
    public void ClickStartBtn()
    {
        if (GameObject.Find("StartButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            Invoke("StageMenu", 0.5f);
            GameObject.Find("StartButton").transform.localScale = new Vector2(1f, 1f);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
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
    public void BackToStartMenu()
    {
        if (GameObject.Find("BackButton").transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            GameObject.Find("BackButton").transform.localScale = new Vector2(1f, 1f);
            stageMenu.SetActive(false);
            startMenu.SetActive(true);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            GameObject.Find("BackButton").transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }
    #endregion


    public void LoadGame() //�ΰ�������
    {
        int num = Data.saveData.gameData.stage;
        string btn = "Stage" + (num+1).ToString();
        
        if (GameObject.Find(btn).transform.localScale.x == 1.2f) //�ι�° Ŭ���̸�
        {
            GameObject.Find(btn).transform.localScale = new Vector2(1f, 1f);
            SceneManager.LoadScene(SceneToLoadGame);
        }
        else //ù��° Ŭ���̸� Ȯ��, ���õ� ȿ��
        {
            GameObject.Find(btn).transform.localScale = new Vector2(1.2f, 1.2f);
            //+���õ� ȿ��
        }
    }

    public void LoadCutScene() //�ƾ�����
    {
        SceneManager.LoadScene(SceneToLoadCutScene);
    }
    public void canvasSkip()
    {
        CanvasEffect.FadeoutSkip();
    }

}

