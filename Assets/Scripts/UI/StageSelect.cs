using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    
    DataManager Data;

    Button stageButton;
    public int stageNum; //스테이지 번호 0번부터
    public GameObject cutSceneButton;
    
    // Start is called before the first frame update
    void Start()
    {
        Data = DataManager.data; //static data
        stageButton = this.transform.GetComponent<Button>();
        //cutScene = transform.FindChild("cutSceneButton");

        Data.saveData.gameData.stage = stageNum; //DataManger에 스테이지 번호 저장

        if (Data.saveData.gameData.stageClearInfo == null) //게임 처음 시작 시
        {
            Data.saveData.gameData.stageClearInfo.Add(false);
        }
        else
        {
            if (Data.saveData.gameData.stageClearInfo[Data.saveData.gameData.stage] == true) //클리어 완료 시
            {
                stageButton.interactable = true; //스테이지 활성화
                cutSceneButton.SetActive(true); //컷씬 버튼 활성화
            }
            else //스테이지 클리어 X
            {
                if (Data.saveData.gameData.stage == 0) //첫번째 스테이지는 항상 활성화
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

}
