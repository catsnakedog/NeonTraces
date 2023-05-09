using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    
    DataManager Data;
    GameData gamedata;

    Button stageButton;
    public static int stageNum; //�������� ��ȣ 0������
    public GameObject cutSceneButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        //cutScene = transform.FindChild("cutSceneButton");

        gamedata = Data.saveData.gameData; //�ذ� ��� ��� �ʿ�.....Null Reference
        gamedata.stage = stageNum;

        if (gamedata.stageClearInfo[stageNum] == true) //Ŭ���� �Ϸ� ��
        {
            stageButton.interactable = true; //�������� Ȱ��ȭ
            cutSceneButton.SetActive(true); //�ƾ� ��ư Ȱ��ȭ
        }
        else //�������� Ŭ���� X
        {
            if (stageNum == 0) //ù��° ���������� �׻� Ȱ��ȭ 
            {
                stageButton.interactable = true;
            }
            else //�������� Ŭ�� �Ұ�
            {
                stageButton.interactable = false;
            }
            cutSceneButton.SetActive(false); //�ƾ� ��ư ��Ȱ��ȭ
        }
    }

}
