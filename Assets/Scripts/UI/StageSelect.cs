using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    
    DataManager Data;

    Button stageButton;
    public int stageNum; //�������� ��ȣ 0������
    public GameObject cutSceneButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        //cutScene = transform.FindChild("cutSceneButton");

        Data.saveData.gameData.stage = stageNum; //DataManger�� �������� ��ȣ ����

        if (Data.saveData.gameData.stageClearInfo == null) //���� ó�� ���� ��
        {
            Data.saveData.gameData.stageClearInfo.Add(false);
        }
        else
        {
            if (Data.saveData.gameData.stageClearInfo[Data.saveData.gameData.stage] == true) //Ŭ���� �Ϸ� ��
            {
                stageButton.interactable = true; //�������� Ȱ��ȭ
                cutSceneButton.SetActive(true); //�ƾ� ��ư Ȱ��ȭ
            }
            else //�������� Ŭ���� X
            {
                if (Data.saveData.gameData.stage == 0) //ù��° ���������� �׻� Ȱ��ȭ
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

}
