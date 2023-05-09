using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    
    DataManager Data;
    GameData gamedata;

    Button stageButton;
    public static int stageNum; //스테이지 번호 0번부터
    public GameObject cutSceneButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        //cutScene = transform.FindChild("cutSceneButton");

        gamedata = Data.saveData.gameData; //해결 방안 모색 필요.....Null Reference
        gamedata.stage = stageNum;

        if (gamedata.stageClearInfo[stageNum] == true) //클리어 완료 시
        {
            stageButton.interactable = true; //스테이지 활성화
            cutSceneButton.SetActive(true); //컷씬 버튼 활성화
        }
        else //스테이지 클리어 X
        {
            if (stageNum == 0) //첫번째 스테이지는 항상 활성화 
            {
                stageButton.interactable = true;
            }
            else //나머지는 클릭 불가
            {
                stageButton.interactable = false;
            }
            cutSceneButton.SetActive(false); //컷씬 버튼 비활성화
        }
    }

}
