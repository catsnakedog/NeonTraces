using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject startMenu;
    public GameObject stageMenu;
    public string SceneToLoadGame; //�ΰ��� ��
    public string SceneToLoadCutScene; //�� ��
    Sound_Manager sm; //�׽�Ʈ

    public GameObject loading;
    private CanvasGroup canvasGroup;
    public float fadeTime;

    private void Start()
    {
        sm = Sound_Manager.sound;
        loading.SetActive(true);    //�ε� ȭ�� Ȱ��ȭ
        startMenu.SetActive(true);  //���� ���۽� ���� �޴� Ȱ��ȭ
        stageMenu.SetActive(false); //���� ���۽� �������� �޴� ��Ȱ��ȭ
        menuPanel.SetActive(false); //���� ���۽� �޴� �г� ��Ȱ��ȭ
        canvasGroup = loading.GetComponent<CanvasGroup>();
    }

    public void SettingOpen()
    {
        sm.Play("Test_ClickSFX");
        Invoke("SettingIn", 0.5f);
    }
    public void SettingIn() //���� �޴� ����
    {
        menuPanel.SetActive(true);
    }

    public void SettingOut() //���� �޴� �ݱ�
    {
        menuPanel.SetActive(false);
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

