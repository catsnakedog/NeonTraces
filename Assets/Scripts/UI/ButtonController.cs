using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject startMenu;
    public GameObject stageMenu;
    
    private void Start()
    {
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

}
