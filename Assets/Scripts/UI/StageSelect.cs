using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    GameObject stage;
    //GameObject cutScene;
 
    DataManager Data;
    GameData gamedata;

    Image image;
    public int stageNum; //�������� ��ȣ 0������
    
    // Start is called before the first frame update
    void Start()
    {
        image = this.transform.GetComponent<Image>();
        //cutScene = transform.FindChild("cutSceneButton");
        
        Data.Load();
        gamedata = Data.saveData.gameData;
        if (gamedata.stageClearInfo[stageNum] == true) //Ŭ���� �Ϸ� ��
        {
            //gameObject.SetActive(true); //�ƾ� ��ư Ȱ��ȭ
        }
        else
        {
            image.color = Color.gray;
            //gameObject.SetActive(false); //�ƾ� ��ư ��Ȱ��ȭ
        }
    }

}
