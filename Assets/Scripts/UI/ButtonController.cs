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
       
        OptionCanvas = soundmanager.optionCanvas;
        OptionCanvas.SetActive(false); //���� ���۽� �޴� �г� ��Ȱ��ȭ
    }

    public void SettingOpen()
    {
        soundmanager.Play("Test_ClickSFX");
        Invoke("SettingIn", 0.5f);
    }
    public void SettingIn() //���� �޴� ����
    {

       OptionCanvas.SetActive(true);
    }

    public void GameExit() //���� ����
    {
#if UNITY_EDITOR //����Ƽ ������
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER //��
         Application.OpenURL("http://google.com");
#else // pc �� ����� ��
        Application.Quit();
#endif
    }

    public void StageMenu() //StartCanvas���� StageCanvas��
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

    public void LoadGame() //�ΰ�������
    {
        SceneManager.LoadScene(SceneToLoadGame);
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

