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

    private void Start()
    {
        startMenu.SetActive(true);  //���� ���۽� ���� �޴� Ȱ��ȭ
        stageMenu.SetActive(false); //���� ���۽� �������� �޴� ��Ȱ��ȭ
        menuPanel.SetActive(false); //���� ���۽� �޴� �г� ��Ȱ��ȭ
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
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
}

